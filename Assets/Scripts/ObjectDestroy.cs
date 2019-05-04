using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour
{
    public GameObject explosion;
    [Range(1, 10)]
    public int multiExplosionCount = 3;
    [Range(1, 10)]
    public float explosionSizeMultiplier = 1;
    [Range(1, 5)]
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
    }

    public void Explode()
    {
        var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(var s in sprites)
        {
            s.enabled = false;
        }
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;

        var collider = gameObject.GetComponent<CircleCollider2D>();
        if (collider != null) collider.enabled = false;

        

        PowerUpManager.Instance.IsPowerUpHit(gameObject.tag);

        if (soundOnDestroy != null && !soundOnDestroy.isPlaying && Random.Range(0.0f, 1.0f) <= 0.5f)
        {
            AudioSource.PlayClipAtPoint(soundOnDestroy.clip, new Vector3(0, 0, 0));
        }

        StartCoroutine(MultipleExplosions());

        var badGuy = gameObject.GetComponentInParent<BadGuyMovement>();
        if (badGuy && !badGuy.isDestroyed)
        {
            badGuy.isDestroyed = true;
            fireControl?.AddScore(destructionValue);
        }
    }

    private IEnumerator MultipleExplosions()
    {
        //force this down to 1 due to performance madness
        multiExplosionCount = 1;

        for (var i = 0; i < multiExplosionCount; i++)
        {
            var pos = transform.position;
            //pos.x += 3 * explosionSizeMultiplier;
            var instance = Instantiate(explosion, pos, Quaternion.identity);
            if (!instance) yield return null;
            instance.transform.localScale *= explosionSizeMultiplier;

            for (var n = 0; n < instance.transform.childCount; n++)
            {
                var childPS = instance.transform.GetChild(n);
                childPS.localScale *= explosionSizeMultiplier;
            }

            Destroy(instance, 1.5f);
            yield return new WaitForSeconds(multiExplosionIntervalDelay);
        }
    }
}
