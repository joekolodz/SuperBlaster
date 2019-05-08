using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaDestroy : MonoBehaviour
{
    [Range(0, 3)]
    public float explosionScale = 1.0f;
    public AudioSource audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!audioSource.isPlaying && audioSource.enabled)
        {
            audioSource.Play();
        }

        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform, explosionScale));

        //hide the plasma blast sprite so the sound can still play
        //don't hide the particles coz it looks cool
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        var trail = gameObject.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.Clear();
        }

        //StartCoroutine(WaitForTime.Wait(2.0f, () => { ObjectPooler.Instance.ReturnPlasma(gameObject); }));
        ObjectPooler.Instance.ReturnPlasma(gameObject);
    }
}
