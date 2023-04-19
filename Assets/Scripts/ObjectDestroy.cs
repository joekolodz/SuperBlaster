using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public bool IsPooledObject = false;


    private static DateTime LastTimeSoundPlayed = DateTime.Now;

    public void Explode(float delayDestroy)
    {
        var parent = gameObject.transform.parent.gameObject;
        var bgm = parent.GetComponent<BadGuyMovement>();
        if (bgm && bgm.isDestroyed) return;

        var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sprites)
        {
            s.enabled = false;
        }

        var collider = gameObject.GetComponent<CircleCollider2D>();
        if (collider != null) collider.enabled = false;

        var rand = Random.Range(0.0f, 1.0f);
        if (soundOnDestroy != null && !soundOnDestroy.isPlaying && rand <= 0.3f && DateTime.Now > ObjectDestroy.LastTimeSoundPlayed.AddSeconds(7))
        {
            var volume = 0.1f;
            AudioSource.PlayClipAtPoint(soundOnDestroy.clip, new Vector3(0, 0, 0), volume);
            ObjectDestroy.LastTimeSoundPlayed = DateTime.Now;
        }

        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform, explosionSizeMultiplier, false));

        if (bgm)
        {
            EventAggregator.PublishBadGuyDied(new BadGuyDiedEventArgs(bgm.gameObject, destructionValue));
        }

        if (bgm && !bgm.isDestroyed)
        {
            //if this object is pooled it shouldn't really be marked destroyed. all bad guys dead are detected a different way
            bgm.isDestroyed = true;
        }

        if (IsPooledObject) return;

        delayDestroy = 0;//i dont think this is needed anymore and anyway it causes a delay with ShieldState/CameraShake
        Destroy(gameObject, delayDestroy);
    }
}
