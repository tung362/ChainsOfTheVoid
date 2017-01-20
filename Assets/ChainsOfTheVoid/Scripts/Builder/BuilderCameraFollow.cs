using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderCameraFollow : MonoBehaviour
{
    public float ScrollSpeed = 5;
    public float ZoomSpeed = 3;
    public float ZoomRate = 0.5f;
    public float ZoomMin = 0.79f;
    public float ZoomMax = 10.79f;
    private float newMouseZ = 0;


    void Start()
    {
        newMouseZ = transform.position.z;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) transform.position = transform.position + (new Vector3(0, 1, 0) * ScrollSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow)) transform.position = transform.position + (new Vector3(0, -1, 0) * ScrollSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftArrow)) transform.position = transform.position + (new Vector3(-1, 0, 0) * ScrollSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow)) transform.position = transform.position + (new Vector3(1, 0, 0) * ScrollSpeed * Time.deltaTime);

        //Zoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
        {
            newMouseZ -= ZoomRate;
            if (newMouseZ < ZoomMin) newMouseZ = ZoomMin;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            newMouseZ += ZoomRate;
            if (newMouseZ > ZoomMax) newMouseZ = ZoomMax;
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, newMouseZ), ScrollSpeed * Time.deltaTime);
    }
}
