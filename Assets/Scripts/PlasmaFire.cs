using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaFire : MonoBehaviour
{
    public float plasmaForce = 3000;
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FirePoint"))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.right * plasmaForce);

            if (audioSource.isPlaying) return;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
    }

}
