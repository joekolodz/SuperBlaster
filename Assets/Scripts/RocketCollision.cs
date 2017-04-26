using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCollision : MonoBehaviour
{
    public GameObject rocketExplosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        
        if (collision.gameObject.name == "Bad Guy")
        {
            Destroy(collision.gameObject);
            //var explosionInstance2 = Instantiate(explosion, transform.position, Quaternion.identity);
        }

        var explosionInstance = Instantiate(rocketExplosion, transform.position, Quaternion.identity);
        Destroy(explosionInstance, 1.0f);
    }
}
