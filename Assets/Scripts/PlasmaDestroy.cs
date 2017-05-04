using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaDestroy : MonoBehaviour
{
    public GameObject explosion;
    [Range(0,3)]
    public float explosionScale = 1.0f;
    public AudioSource audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(audioSource != null && !audioSource.isPlaying)
            audioSource.Play();

        //hide the plasma blast sprite so the sound can still play
        //don't hide the particles coz it looks cool
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        Destroy(gameObject, 2.0f);
    }

    private void OnDestroy()
    {
        var explosion = Instantiate(this.explosion, transform.position, Quaternion.identity);
        explosion.GetComponentInChildren<ParticleSystem>().transform.localScale *= explosionScale;
        Destroy(explosion, 1.0f);
    }
}
