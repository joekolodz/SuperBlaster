using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    public GameObject explosion;
    [Range(1, 10)]
    public int multiExplosionSize = 3;
    [Range(1, 10)]
    public int explosionSizeMultiplier = 1;
    public float multiExplosionIntervalDelay = 0.3f;

    public AudioSource soundOnDestroy;
    public AudioSource soundYeah;

    private bool isClipPlaying = false;

    public void Explode()
    {
        if(soundOnDestroy!=null && !isClipPlaying)
        {
            isClipPlaying = true;
            AudioSource.PlayClipAtPoint(soundYeah.clip, new Vector3());
            AudioSource.PlayClipAtPoint(soundOnDestroy.clip, new Vector3());
        }

        StartCoroutine(MultipleExplosions());
    }

    private IEnumerator MultipleExplosions()
    {
        for (var i = 0; i < multiExplosionSize; i++)
        {
            var instance = Instantiate(explosion, transform.position, Quaternion.identity);
            instance.transform.localScale *= explosionSizeMultiplier;
            var ps = instance.transform.Find("PS Explosion");
            ps.localScale *= explosionSizeMultiplier;
            Destroy(instance, 1.5f);
            yield return new WaitForSeconds(multiExplosionIntervalDelay);
        }
    }
}
