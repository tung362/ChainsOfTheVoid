using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMovement : MonoBehaviour {

    public float pulseSpeed, inputAngle;
    private float Timer;

	// Use this for initialization
	void Start () {

        Timer = pulseSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        Timer -= Time.deltaTime;

        if (Timer <= 0.0f) Timer = pulseSpeed;

        RotateTrans();
	}

    void RotateTrans()
    {
        this.transform.Rotate(Vector3.up, inputAngle * Time.deltaTime * (Timer * 2));
    }
}
