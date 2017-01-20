using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform  shotSpawn;
    public GameObject shot;
    private Transform target;

    private float nextFire =  .5f;
    public  float fireRate = .75f;

    private bool canShoot = false;

    private int projectileForce = 750;

   void OnTriggerEnter(Collider col)
   {
        Debug.Log("Enemy in range!!!");
        if (col.gameObject.CompareTag("Enemy"))
        {
            target = col.gameObject.transform;
            canShoot = true;
        }
   }

    void OnTriggerStay(Collider col)
    {
        if(/*canShoot == true && */col.gameObject.CompareTag("Enemy"))
        {
            shotSpawn.LookAt(target);
            InvokeRepeating("Shoot", 0.1f, 1);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (canShoot == true && col.gameObject.CompareTag("Enemy"))
        {
            canShoot = false;
        }
    }

    void Shoot()
    {
        if (canShoot == true && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject clone = (GameObject)Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            Vector3 force = shotSpawn.forward * projectileForce;
            clone.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
        }
    }
}
