using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disconnect explosions from rockets/plasma. If rockets are returned to the pool (made inactive) before the explosion finishes and is returned to the pool, then the explosion will not be returned to the pool and cleaned up
/// </summary>
public class Explosions : MonoBehaviour
{
    public static readonly Explosions Instance = (new GameObject("ExplosionsSingletonContainer")).AddComponent<Explosions>();

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
        EventAggregator.ObjectDestroyed += EventAggregator_ObjectDestroyed;
    }

    private void EventAggregator_ObjectDestroyed(object sender, ObjectDestroyedEventArgs e)
    {
        var explosion = ObjectPooler.Instance.GetExplosionSmall();
        if (explosion)
        {
            explosion.transform.position = e.Transform.position;
            explosion.transform.rotation = Quaternion.identity;
            var ps = explosion.GetComponentInChildren<ParticleSystem>();
            var psScaleBefore = ps.transform.localScale;
            ps.transform.localScale *= e.ExplosionScale;
            //Debug.Log($"Before: localScale: {psScaleBefore.ToString()} After: ExplosionScale:{e.ExplosionScale}, localScale:{ps.transform.localScale.ToString()}");
            explosion.SetActive(true);
            StartCoroutine(WaitForTime.Wait(1.0f, () => { ObjectPooler.Instance.ReturnExplosionSmall(explosion); ps.transform.localScale = psScaleBefore; }));
        }
    }

}
