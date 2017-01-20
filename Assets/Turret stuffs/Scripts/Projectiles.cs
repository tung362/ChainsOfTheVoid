using UnityEngine;
using System.Collections;

public class Projectiles : MonoBehaviour
{
    //public int damage = 20;
    public float lifetime = 5f;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Enemy"))
            Destroy(gameObject);
    }
}