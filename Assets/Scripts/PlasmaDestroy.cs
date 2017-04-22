using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaDestroy : MonoBehaviour
{
    public Transform plasma;
    public GameObject explosion;
    public float collisionRadius = 0.4f;
    public bool collided = false;
    public LayerMask whatToCollidWith;

    void Start()
    {
        //in case it never hits anything, the plasma self explodes!
        Destroy(gameObject, Random.Range(0.25f, 2.0f));
    }

    private void FixedUpdate()
    {

        if (collided)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        var explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
        var body = explosion.AddComponent<Rigidbody2D>();
        body.AddForce(plasma.right * 1000);
        body.gravityScale = 0;
        Destroy(explosion, 0.275f);
    }
}
