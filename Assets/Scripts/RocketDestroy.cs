using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDestroy : MonoBehaviour
{
    public GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroy the rocket anytime it hits anything
        Destroy(gameObject);
        var explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
        Destroy(explosion, 1.0f);
    }
}
