using UnityEngine;
using System.Collections;

public class ParticleRotationToParent : MonoBehaviour
{
    private ParticleSystem TheParticleSystem;

    void Start ()
    {
        TheParticleSystem = GetComponent<ParticleSystem>();
    }
	
	void Update ()
    {
        TheParticleSystem.startRotation3D = transform.root.rotation.eulerAngles * Mathf.Deg2Rad;
    }
}
