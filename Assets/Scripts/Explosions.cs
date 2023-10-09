using UnityEngine;

public class Explosions : MonoBehaviour
{
    private GameObject _explosionSmallPrefab;
    private GameObject _explosionLargePrefab;
    private GameObject _debrisPrefab;

    public void Awake()
    {
        _explosionSmallPrefab = (GameObject)Resources.Load("prefabs/Explosion Small");
        _explosionLargePrefab = (GameObject)Resources.Load("prefabs/Explosion Large");
        _debrisPrefab = (GameObject)Resources.Load("prefabs/PS Debris");

        EventAggregator.ObjectDestroyed += EventAggregator_ObjectDestroyed;
        EventAggregator.ShowDebris += EventAggregator_ShowDebris;
    }

    private void EventAggregator_ObjectDestroyed(object sender, ObjectDestroyedEventArgs e)
    {
        if (e.IsSmallExplosion)
        {
            SmallExplosion(e);
        }
        else
        {
            LargeExplosions(e);
        }
    }

    private void SmallExplosion(ObjectDestroyedEventArgs e)
    {
        var explosion = Instantiate(_explosionSmallPrefab);

        explosion.transform.SetPositionAndRotation(e.Transform.position, Quaternion.identity);
        var ps = explosion.GetComponentInChildren<ParticleSystem>();
        var psScaleBefore = ps.transform.localScale;
        ps.transform.localScale *= e.ExplosionScale;
        explosion.SetActive(true);

        Destroy(explosion, 0.5f);
    }

    private void LargeExplosions(ObjectDestroyedEventArgs e)
    {
        var explosion = Instantiate(_explosionLargePrefab);

        explosion.transform.SetPositionAndRotation(e.Transform.position, Quaternion.identity);
        var ps = explosion.GetComponentInChildren<ParticleSystem>();
        var psScaleBefore = ps.transform.localScale;
        ps.transform.localScale *= e.ExplosionScale;

        for (var n = 0; n < explosion.transform.childCount; n++)
        {
            var childPS = explosion.transform.GetChild(n);
            childPS.localScale *= e.ExplosionScale;
        }

        explosion.SetActive(true);
        
        Destroy(explosion, 1.0f);
    }

    private void EventAggregator_ShowDebris(object sender, ShowDebrisEventArgs e)
    {
        var debris = Instantiate(_debrisPrefab);

        if (debris == null) return;
        debris.transform.position = e.Transform.position;
        debris.SetActive(true);
        debris.GetComponentInChildren<ParticleSystem>().Play();

        Destroy(debris, 1.0f);
    }
}