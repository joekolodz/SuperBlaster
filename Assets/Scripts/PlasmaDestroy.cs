using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaDestroy : MonoBehaviour
{
    public GameObject explosion;
    [Range(0,3)]
    public float explosionScale = 1.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        var explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
        explosion.GetComponentInChildren<ParticleSystem>().transform.localScale *= explosionScale;
        Destroy(explosion, 1.0f);
    }
}
