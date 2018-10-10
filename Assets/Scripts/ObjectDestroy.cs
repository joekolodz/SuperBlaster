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
    public int destructionValue;
    public AudioSource soundOnDestroy;

    private FireControl fireControl;

    private void Start()
    {
        var fc = GameObject.Find("FireControl");
        if (fc)
        {
            fireControl = fc.GetComponent<FireControl>();
        }
        if (fireControl == null)
        {
            Debug.Log("Can't find the fireControl script");
        }
    }

    public void Explode()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        if (soundOnDestroy != null && !soundOnDestroy.isPlaying)
        {
            soundOnDestroy.Play();
        }
        
        StartCoroutine(MultipleExplosions());

        var badGuy = gameObject.GetComponentInParent<BadGuyMovement>();
        if(badGuy && !badGuy.isDestroyed)
        {
            badGuy.isDestroyed = true;
            fireControl.AddScore(destructionValue);
        }
    }

    private IEnumerator MultipleExplosions()
    {
        for (var i = 0; i < multiExplosionSize; i++)
        {
            var pos = transform.position;
            pos.x += 3 * explosionSizeMultiplier;
            var instance = Instantiate(explosion, pos, Quaternion.identity);            
            instance.transform.localScale *= explosionSizeMultiplier;
            var ps = instance.transform.Find("PS Explosion");
            ps.localScale *= explosionSizeMultiplier;
            Destroy(instance, 1.5f);
            yield return new WaitForSeconds(multiExplosionIntervalDelay);
        }
    }
}
