using Assets.Scripts;
using UnityEngine;

/// <summary>
/// Disconnect explosions from rockets/plasma. If rockets are returned to the pool (made inactive) before the explosion finishes and is returned to the pool, then the explosion will not be returned to the pool and cleaned up
/// </summary>
public class Explosions : BaseSingleton<Explosions>
{
    private static bool _isInitialized = false;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit. this enforces that the class is initialized only when the first static member is accessed
    static Explosions()
    {
    }
    //private constructor is called before static constructor
    private Explosions()
    {
    }

    public void Initialize()
    {
        if (_isInitialized) return;
        EventAggregator.ObjectDestroyed += EventAggregator_ObjectDestroyed;
        EventAggregator.ShowDebris += EventAggregator_ShowDebris;
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
            StartCoroutine(WaitForTime.Wait(1.0f, () =>
            {
                var ex1 = explosion;
                ObjectPooler.Instance.ReturnExplosionSmall(ex1); ps.transform.localScale = psScaleBefore;
            }));
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
            StartCoroutine(WaitForTime.Wait(2.0f, () =>
            {
                var ex1 = explosion;
                ObjectPooler.Instance.ReturnExplosionLarge(ex1); ps.transform.localScale = psScaleBefore;
            }));
        }
    }

    private void EventAggregator_ShowDebris(object sender, ShowDebrisEventArgs e)
    {
        var debris = ObjectPooler.Instance.GetDebris();
        if (debris is null) return;
        debris.transform.position = e.Transform.position;
        debris.SetActive(true);
        debris.GetComponentInChildren<ParticleSystem>().Play();

        StartCoroutine(WaitForTime.Wait(2.0f, () => { ObjectPooler.Instance.ReturnDebris(debris); }));
    }
}
