using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCollision : MonoBehaviour
{
    public GameObject rocketExplosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Yup");

        Destroy(gameObject);
        
        if (collision.gameObject.name == "Bad Guy")
        {
            Destroy(collision.gameObject);

        }

        var explosionInstance = Instantiate(rocketExplosion, transform.position, Quaternion.identity);
        Destroy(explosionInstance, 1.0f);
    }
}
