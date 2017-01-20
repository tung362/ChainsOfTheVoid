using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyStates 
{
    private readonly EnemyStatePattern enemy;
    private int nextWaypoint;

    public PatrolState(EnemyStatePattern enemyStatePattern) // Constructor
    {
        enemy = enemyStatePattern;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            ToChaseState();
    }

    public void ToPatrolState()
    {
        Debug.Log("Cannot transition to current state (Patrol)");
    }

    public void ToChaseState()
    {
        //enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.raycastOrigin.transform.position, enemy.raycastOrigin.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            //enemy.chaseTarget = hit.transform; // Start chasing player if sighted
            //ToChaseState();
            Debug.Log("Target Sighted");
        }
    }

    void Patrol()
    {
        enemy.nma.destination = enemy.waypoints[nextWaypoint].position;
        enemy.nma.Resume();

        if (enemy.nma.remainingDistance <= enemy.nma.stoppingDistance && !enemy.nma.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % enemy.waypoints.Length;
        }
    }
}