using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disconnect explosions from rockets/plasma. If rockets are returned to the pool (made inactive) before the explosion finishes and is returned to the pool, then the explosion will not be returned to the pool and cleaned up
/// </summary>
public class Explosions : MonoBehaviour
{
    public static readonly Explosions Instance = (new GameObject("ExplosionsSingletonContainer")).AddComponent<Explosions>();

    private static bool _isInitialized = false;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static Explosions()
    {

    }
    private Explosions()
    {

    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        if (_isInitialized) return;
        EventAggregator.ObjectDestroyed += EventAggregator_ObjectDestroyed;
        _isInitialized = true;
    }

    private void EventAggregator_ObjectDestroyed(object sender, ObjectDestroyedEventArgs e)
    {
        GameObject explosion;
        if (e.IsSmallExplosion)
        {
            explosion = ObjectPooler.Instance.GetExplosionSmall();
            SmallExplosion(e, explosion);
        }
        else
        {
            explosion = ObjectPooler.Instance.GetExplosionLarge();
            LargeExplosions(e, explosion);
        }
    }

    private void SmallExplosion(ObjectDestroyedEventArgs e, GameObject explosion)
    {
        if (explosion)
        {
            explosion.transform.position = e.Transform.position;
            explosion.transform.rotation = Quaternion.identity;
            var ps = explosion.GetComponentInChildren<ParticleSystem>();
            var psScaleBefore = ps.transform.localScale;
            ps.transform.localScale *= e.ExplosionScale;
            explosion.SetActive(true);
            StartCoroutine(WaitForTime.Wait(1.0f, () => { ObjectPooler.Instance.ReturnExplosionSmall(explosion); ps.transform.localScale = psScaleBefore; }));
        }
    }

    private void LargeExplosions(ObjectDestroyedEventArgs e, GameObject explosion)
    {
        if (explosion)
        {
            explosion.transform.position = e.Transform.position;
            explosion.transform.rotation = Quaternion.identity;
            var ps = explosion.GetComponentInChildren<ParticleSystem>();
            var psScaleBefore = ps.transform.localScale;
            ps.transform.localScale *= e.ExplosionScale;

            for (var n = 0; n < explosion.transform.childCount; n++)
            {
                var childPS = explosion.transform.GetChild(n);
                childPS.localScale *= e.ExplosionScale;
            }

            explosion.SetActive(true);
            StartCoroutine(WaitForTime.Wait(2.0f, () => { ObjectPooler.Instance.ReturnExplosionLarge(explosion); ps.transform.localScale = psScaleBefore; }));
        }
    }

}
