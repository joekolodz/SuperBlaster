using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDestroy : MonoBehaviour
{
    public Transform rocket;
    public GameObject explosion;
    public float collisionRadius = 0.4f;
    public bool collided = false;
    public LayerMask whatToCollidWith;

    private void Start()
    {
        //in case it never hits anything, the rocket self explodes!
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
        //body.AddForce(rocket.right * 1000);
        body.gravityScale = 0;
        Destroy(explosion, 1.0f);
    }
}
