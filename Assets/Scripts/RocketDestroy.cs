using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDestroy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform));

        //var explosion = ObjectPooler.Instance.GetExplosionSmall();
        //if (explosion)
        //{
        //    explosion.transform.position = transform.position;
        //    explosion.transform.rotation = Quaternion.identity;
        //    explosion.SetActive(true);
        //    StartCoroutine(WaitForTime.Wait(1.0f, () => { ObjectPooler.Instance.ReturnExplosionSmall(explosion); }));
        //}

        //var collider = gameObject.GetComponent<PolygonCollider2D>();
        //if (collider != null) collider.enabled = false;

        //var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        //foreach (var s in sprites)
        //{
        //    s.enabled = false;
        //}

        var trail = gameObject.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.Clear();
        }

        //StartCoroutine(WaitForTime.Wait(2.0f, () => ResetRocket(gameObject)));
        //StartCoroutine(WaitForTime.Wait(1.1f, () => ResetRocket(gameObject, collider, sprites, trail)));
        ObjectPooler.Instance.ReturnRocket(gameObject);
    }

    private void ResetRocket(GameObject gameObject)
    {
        ObjectPooler.Instance.ReturnRocket(gameObject);
    }

    private void ResetRocket(GameObject gameObject, PolygonCollider2D collider, SpriteRenderer[] sprites, TrailRenderer trail)
    {
        if (collider)
        {
            collider.enabled = true;
        }

        if (sprites.Length > 0)
        {
            foreach (var s in sprites)
            {
                s.enabled = true;
            }
        }

        if (trail != null)
        {
            trail.enabled = true;
        }

        ObjectPooler.Instance.ReturnRocket(gameObject);
    }
}
