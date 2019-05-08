using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static readonly ObjectPooler Instance = (new GameObject("ObjectPoolerSingletonContainer")).AddComponent<ObjectPooler>();

    private bool IsPoolPopulated = false;

    private GameObject ExplosionLargePool;
    private int ExplosionLargePoolSize = 5;
    private GameObject ExplosionLargePrefab;
    private List<GameObject> _explosionLargePool;

    private GameObject ExplosionSmallPool;
    private int ExplosionSmallPoolSize = 50;
    private GameObject ExplosionSmallPrefab;
    private List<GameObject> _explosionSmallPool;

    private GameObject RocketPool;
    private int RocketPoolSize = 30;
    private GameObject RocketPrefab;
    private List<GameObject> _rocketPool;

    private GameObject PlasmaPool;
    private int PlasmaPoolSize = 30;
    private GameObject PlasmaPrefab;
    private List<GameObject> _plasmaPool;


    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static ObjectPooler()
    {

    }
    private ObjectPooler()
    {

    }

    private void Awake()
    {
        RocketPrefab = (GameObject)Resources.Load("prefabs/Rocket");
        RocketPool = new GameObject("Bullet Pool");

        PlasmaPrefab = (GameObject)Resources.Load("prefabs/Plasma Blast");
        PlasmaPool = new GameObject("Plasma Pool");

        ExplosionLargePrefab = (GameObject)Resources.Load("prefabs/Explosion Large");
        ExplosionLargePool = new GameObject("Explosion-Large Pool");

        ExplosionSmallPrefab = (GameObject)Resources.Load("prefabs/Explosion Small");
        ExplosionSmallPool = new GameObject("Explosion-Small Pool");

        _rocketPool = new List<GameObject>();
        _plasmaPool = new List<GameObject>();
        _explosionLargePool = new List<GameObject>();
        _explosionSmallPool = new List<GameObject>();

        RocketPool.transform.SetParent(gameObject.transform, true);
        PlasmaPool.transform.SetParent(gameObject.transform, true);
        ExplosionLargePool.transform.SetParent(gameObject.transform, true);
        ExplosionSmallPool.transform.SetParent(gameObject.transform, true);


        PopulatePools();
        DontDestroyOnLoad(gameObject);
    }

    public void PopulatePools()
    {
        if (IsPoolPopulated) return;

        for (var i = 0; i < RocketPoolSize; i++)
        {
            var r = Instantiate(RocketPrefab);
            r.SetActive(false);
            r.transform.SetParent(RocketPool.transform, true);
            _rocketPool.Add(r);
        }

        for (var i = 0; i < PlasmaPoolSize; i++)
        {
            var r = Instantiate(PlasmaPrefab);
            r.SetActive(false);
            r.transform.SetParent(PlasmaPool.transform, true);
            _plasmaPool.Add(r);
        }

        for (var i = 0; i < ExplosionLargePoolSize; i++)
        {
            var r = Instantiate(ExplosionLargePrefab);
            r.SetActive(false);
            r.transform.SetParent(ExplosionLargePool.transform, true);
            _explosionLargePool.Add(r);
        }

        for (var i = 0; i < ExplosionSmallPoolSize; i++)
        {
            var r = Instantiate(ExplosionSmallPrefab);
            r.SetActive(false);
            r.transform.SetParent(ExplosionSmallPool.transform, true);
            _explosionSmallPool.Add(r);
        }

        IsPoolPopulated = true;
    }

    public GameObject GetRocket()
    {
        if (_rocketPool.Count == 0) return null;

        var index = _rocketPool.Count - 1;
        var r = _rocketPool[index];
        _rocketPool.RemoveAt(index);
        return r;
    }

    public void ReturnRocket(GameObject rocket)
    {
        rocket.SetActive(false);
        rocket.transform.position = Vector3.zero;
        _rocketPool.Add(rocket);
    }

    public GameObject GetPlasma()
    {
        if (_plasmaPool.Count == 0) return null;

        var index = _plasmaPool.Count - 1;
        var r = _plasmaPool[index];
        _plasmaPool.RemoveAt(index);
        return r;
    }

    public void ReturnPlasma(GameObject plasma)
    {
        plasma.SetActive(false);
        plasma.transform.position = Vector3.zero;
        _plasmaPool.Add(plasma);
    }

    public GameObject GetExplosionLarge()
    {
        if (_explosionLargePool.Count == 0) return null;

        var index = _explosionLargePool.Count - 1;
        var r = _explosionLargePool[index];
        _explosionLargePool.RemoveAt(index);
        return r;
    }

    public void ReturnExplosionLarge(GameObject explosion)
    {
        explosion.SetActive(false);
        explosion.transform.position = Vector3.zero;
        _explosionLargePool.Add(explosion);
    }

    public GameObject GetExplosionSmall()
    {
        if (_explosionSmallPool.Count == 0) return null;
        var index = _explosionSmallPool.Count - 1;
        var r = _explosionSmallPool[index];
        _explosionSmallPool.RemoveAt(index);
        return r;
    }

    public void ReturnExplosionSmall(GameObject explosion)
    {
        explosion.SetActive(false);
        explosion.transform.position = Vector3.zero;
        _explosionSmallPool.Add(explosion);
    }
}
