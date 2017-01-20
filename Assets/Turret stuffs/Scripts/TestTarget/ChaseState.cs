using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyStates
{
    private readonly EnemyStatePattern enemy;

    public ChaseState(EnemyStatePattern enemyStatePattern) // Constructor
    {
        enemy = enemyStatePattern;
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {
        // end chase timer, after player is outside raycast range for a certain amount of time go back to patrolling
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToChaseState()
    {
        Debug.Log("Cannot transition to current state (Chase)");
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.raycastOrigin.transform.position, enemy.raycastOrigin.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform; // Start chasing player if sighted
            ToChaseState();
        }
        else
        {
            ToPatrolState();
        }
    }

    private void Chase()
    {
        enemy.nma.destination = enemy.chaseTarget.position;
        enemy.nma.Resume();
    }
}