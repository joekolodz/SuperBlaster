using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    public void Awake()
    {
        EventAggregator.ObjectDestroyed += EventAggregator_ObjectDestroyed;
        EventAggregator.ShowDebris += EventAggregator_ShowDebris;
    }

    private void OnDestroy()
    {
        EventAggregator.ObjectDestroyed -= EventAggregator_ObjectDestroyed;
        EventAggregator.ShowDebris -= EventAggregator_ShowDebris;
    }

    private void EventAggregator_ObjectDestroyed(object sender, ObjectDestroyedEventArgs e)
    {
        if (e.IsSmallExplosion)
        {
            SmallExplosion(e);
        }
        else
        {
            LargeExplosions(e.Transform.position, e.ExplosionScale);
        }
    }

    private void SmallExplosion(ObjectDestroyedEventArgs e)
    {
        var explosion = ObjectPooler.Instance.GetExplosionSmall();
        if (explosion == null) return;

        explosion.transform.SetPositionAndRotation(e.Transform.position, Quaternion.identity);
        var ps = explosion.GetComponentInChildren<ParticleSystem>();
        var psScaleBefore = ps.transform.localScale;
        ps.transform.localScale *= e.ExplosionScale;
        explosion.SetActive(true);
    }

    public void LargeExplosions(Vector3 position, float scale)
    {
        var explosion = ObjectPooler.Instance.GetExplosionLarge();
        if (explosion == null) return;

        explosion.transform.SetPositionAndRotation(position, Quaternion.identity);
        var ps = explosion.GetComponentInChildren<ParticleSystem>();
        var psScaleBefore = ps.transform.localScale;
        ps.transform.localScale *= scale;

        for (var n = 0; n < explosion.transform.childCount; n++)
        {
            var childPS = explosion.transform.GetChild(n);
            childPS.localScale *= scale;
        }
        explosion.SetActive(true);
    }

    private void EventAggregator_ShowDebris(object sender, ShowDebrisEventArgs e)
    {
        var debris = ObjectPooler.Instance.GetDebris();
        if (debris == null) return;

        debris.transform.position = e.Transform.position;
        debris.SetActive(true);
        debris.GetComponentInChildren<ParticleSystem>().Play();
    }
}