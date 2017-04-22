using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
    public float rocketForce = 750.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FirePoint")
            GetComponent<Rigidbody2D>().AddForce(transform.right * rocketForce);
    }
}
