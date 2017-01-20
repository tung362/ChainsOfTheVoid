using UnityEngine;
using System.Collections;

public interface IEnemyStates
{
    void UpdateState();

    void OnTriggerEnter(Collider other);

    void ToPatrolState();

    void ToChaseState();
}
