using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDemo : MonoBehaviour
{
    public Vector3 Target = Vector3.zero;
    public float Speed = 20;
    private Vector3 StartingPosition = Vector3.zero;

    void Start()
    {
        StartingPosition = transform.position;
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space)) transform.position = StartingPosition;

        transform.position = Vector3.MoveTowards(transform.position, Target, Speed * Time.fixedDeltaTime);
    }
}
