using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFire : MonoBehaviour
{
    public static bool PowerUp = false;

    public float rocketForce = 3000.0f;
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FirePoint"))
        {
            if(PowerUp)
            {
                rocketForce = 5000;
            }

            GetComponent<Rigidbody2D>().AddForce(transform.right * rocketForce);

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
