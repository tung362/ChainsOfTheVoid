using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is a quick debuging movement script so I can test other features
public class debugMovement : MonoBehaviour {

    [Tooltip("Player's Rigidbody")]
    private Rigidbody rb;
    public  Rigidbody inputRB;
	// Use this for initialization
	void Start () {
        rb = inputRB;
	}
	
	// Update is called once per frame
	void Update () {
        KeyboardMovement();
	}

    void KeyboardMovement()
    {
        //Backwards and Forwards
        if (Input.GetKey(KeyCode.W)) rb.AddRelativeForce(new Vector3(0, 0,  1.0f));
        if (Input.GetKey(KeyCode.S)) rb.AddRelativeForce(new Vector3(0, 0, -1.0f));

        //Left and Right
        if (Input.GetKey(KeyCode.A)) rb.AddRelativeTorque(new Vector3(0,  -2.0f,0));
        if (Input.GetKey(KeyCode.D)) rb.AddRelativeTorque(new Vector3(0,   2.0f,0));

    }
}
