using UnityEngine;
using System.Collections;

public class EnemyStatePattern : MonoBehaviour
{
    public float sightRange = 20f; // Length of raycast for seeing player
    public Transform[] waypoints;
    public Transform raycastOrigin;

    [HideInInspector] public Transform    chaseTarget; // Player
    [HideInInspector] public IEnemyStates currentState;
    [HideInInspector] public PatrolState  patrolState;
    [HideInInspector] public ChaseState   chaseState;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent nma;

    private void Awake()
    {
        patrolState = new PatrolState(this);
        chaseState = new  ChaseState(this);
        nma = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

	// Use this for initialization
	void Start ()
    {
        currentState = patrolState;
	}
	
	// Update is called once per frame
	public void Update ()
    {
        currentState.UpdateState();
	}

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}