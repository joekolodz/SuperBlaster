using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static readonly ObjectPooler Instance = (new GameObject("ObjectPoolerSingletonContainer")).AddComponent<ObjectPooler>();

    private bool IsPoolPopulated = false;
    private bool IsBadGuyArrowheadPoolPopulated = false;

    private GameObject ExplosionLargePool;
    private int ExplosionLargePoolSize = 10;
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

    private Queue<GameObject> _rocketQueue;


    private GameObject PlasmaPool;
    private int PlasmaPoolSize = 30;
    private GameObject PlasmaPrefab;
    private List<GameObject> _plasmaPool;

    private GameObject LaserPool;
    private int LaserPoolSize = 30;
    private GameObject LaserPrefab;
    private List<GameObject> _laserPool;

    private GameObject BadGuyArrowheadPool;
    //private int BadGuyArrowheadPoolSize = 20;
    private GameObject BadGuyArrowheadPrefab;
    private List<GameObject> _badGuyArrowheadPool;


    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit. this enforces that the class is initialized only when the first static member is accessed
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

        LaserPrefab = (GameObject)Resources.Load("prefabs/Laser Blast");
        LaserPool = new GameObject("Laser Pool");

        ExplosionLargePrefab = (GameObject)Resources.Load("prefabs/Explosion Large");
        ExplosionLargePool = new GameObject("Explosion-Large Pool");

        ExplosionSmallPrefab = (GameObject)Resources.Load("prefabs/Explosion Small");
        ExplosionSmallPool = new GameObject("Explosion-Small Pool");

        BadGuyArrowheadPrefab = (GameObject)Resources.Load("prefabs/Bad Guy Template (Arrowhead)");
        BadGuyArrowheadPool = new GameObject("Bad Guy - Arrowhead Pool");

        _rocketPool = new List<GameObject>();
        _rocketQueue = new Queue<GameObject>();
        _plasmaPool = new List<GameObject>();
        _laserPool = new List<GameObject>();
        _explosionLargePool = new List<GameObject>();
        _explosionSmallPool = new List<GameObject>();
        _badGuyArrowheadPool = new List<GameObject>();

        RocketPool.transform.SetParent(gameObject.transform, true);
        PlasmaPool.transform.SetParent(gameObject.transform, true);
        LaserPool.transform.SetParent(gameObject.transform, true);
        ExplosionLargePool.transform.SetParent(gameObject.transform, true);
        ExplosionSmallPool.transform.SetParent(gameObject.transform, true);
        BadGuyArrowheadPool.transform.SetParent(gameObject.transform, true);

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
            //_rocketPool.Add(r);
            _rocketQueue.Enqueue(r);
        }

        for (var i = 0; i < PlasmaPoolSize; i++)
        {
            var r = Instantiate(PlasmaPrefab);
            r.SetActive(false);
            r.transform.SetParent(PlasmaPool.transform, true);

            _plasmaPool.Add(r);
        }

        for (var i = 0; i < LaserPoolSize; i++)
        {
            var r = Instantiate(LaserPrefab);
            r.SetActive(false);
            r.transform.SetParent(LaserPool.transform, true);
            _laserPool.Add(r);
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

    public void PopulateBadGuyArrowheadPool(int poolSize)
    {
        if (IsBadGuyArrowheadPoolPopulated)
        {
            foreach (var o in _badGuyArrowheadPool)
            {
                //WHAT? ReturnBadGuyArrowhead(o);
                var bgm = o.GetComponent<BadGuyMovement>();
                bgm.Reset();
            }
            return;
        }

        for (var i = 0; i < poolSize; i++)
        {
            var r = Instantiate(BadGuyArrowheadPrefab);
            r.transform.position = new Vector3(60, -5, 0);
            r.name += $"[{i}]";
            r.transform.Find("Bad Guy").GetComponent<ObjectDestroy>().IsPooledObject = true; ;
            r.SetActive(false);
            r.transform.SetParent(BadGuyArrowheadPool.transform, true);
            r.GetComponent<BadGuyMovement>().Id = i + 1;
            _badGuyArrowheadPool.Add(r);
        }
        IsBadGuyArrowheadPoolPopulated = true;
    }

    public void Reset()
    {
        foreach (var o in _badGuyArrowheadPool)
        {
            ReturnBadGuyArrowhead(o);
            var bgm = o.GetComponent<BadGuyMovement>();
            bgm.Reset();
        }

        var allRockets = FindObjectsOfType<RocketFire>();
        foreach (var rocket in allRockets)
        {
            ReturnRocket(rocket.gameObject);
        }

        var allPlasmaBlasts = FindObjectsOfType<PlasmaFire>();
        foreach (var blast in allPlasmaBlasts)
        {
            ReturnPlasma(blast.gameObject);
        }

        var allLaserBlasts = FindObjectsOfType<LaserFire>();
        foreach (var blast in allLaserBlasts)
        {
            ReturnLaser(blast.gameObject);
        }
    }

    public GameObject GetRocket()
    {
        //if (_rocketPool.Count == 0) return null;

        //var index = _rocketPool.Count - 1;
        //var r = _rocketPool[index];
        //_rocketPool.RemoveAt(index);
        //return r;


        if (_rocketQueue.Count == 0) return null;

        var index = _rocketQueue.Count - 1;
        var r = _rocketQueue.Dequeue();
        return r;
    }

    public void ReturnRocket(GameObject rocket)
    {
        rocket.SetActive(false);
        rocket.GetComponent<RocketFire>().StopRocket();

        var rb = rocket.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rocket.transform.position = Vector3.zero;

        gameObject.GetComponentInChildren<TrailRenderer>()?.Clear();

        //_rocketPool.Add(rocket);
        _rocketQueue.Enqueue(rocket);
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
        var rb = plasma.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(-transform.right * 3000);

        plasma.transform.position = Vector3.zero;
        plasma.transform.rotation = Quaternion.identity;
        _plasmaPool.Add(plasma);
    }

    public GameObject GetLaser()
    {
        if (_laserPool.Count == 0) return null;

        var index = _laserPool.Count - 1;
        var r = _laserPool[index];
        _laserPool.RemoveAt(index);
        return r;
    }

    public void ReturnLaser(GameObject laser)
    {
        laser.SetActive(false);
        laser.transform.position = Vector3.zero;
        _laserPool.Add(laser);
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

    public GameObject GetBadGuyArrowhead()
    {
        if (_badGuyArrowheadPool.Count == 0) return null;

        var index = _badGuyArrowheadPool.Count - 1;
        var r = _badGuyArrowheadPool[index];
        _badGuyArrowheadPool.RemoveAt(index);

        var bgm = r.GetComponent<BadGuyMovement>();
        bgm.Reset();

        return r;
    }


    public void ReturnBadGuyArrowhead(GameObject o)
    {
        var bgm = o.GetComponent<BadGuyMovement>();

        var badGuy = bgm.badGuy;
        badGuy.transform.position = new Vector3(100, 100, 0);

        var sprites = badGuy.GetComponentsInChildren<SpriteRenderer>();
        foreach (var s in sprites)
        {
            s.enabled = true;
        }

        var collider = badGuy.GetComponentInChildren<CircleCollider2D>();
        if (collider != null) collider.enabled = true;

        var rb = badGuy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        o.SetActive(false);
        _badGuyArrowheadPool.Add(o);
    }
}
