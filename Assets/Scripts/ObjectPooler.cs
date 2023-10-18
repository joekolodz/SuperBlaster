using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : BaseSingleton<ObjectPooler>
{
    private bool _isPoolPopulated = false;
    private bool _isBadGuyArrowheadPoolPopulated = false;

    private GameObject ExplosionLargePool;
    private int ExplosionLargePoolSize = 10;
    private int ExplosionLargeAlive = 0;
    private GameObject ExplosionLargePrefab;
    private Queue<GameObject> _explosionLargePool;

    private GameObject ExplosionSmallPool;
    private int ExplosionSmallPoolSize = 50;
    private int ExplosionSmallAlive = 0;
    private GameObject ExplosionSmallPrefab;
    private Queue<GameObject> _explosionSmallPool;

    private GameObject DebrisPool;
    private int DebrisPoolSize = 12;
    private int DebrisAlive = 0;
    private GameObject DebrisPrefab;
    private Queue<GameObject> _debrisPool;

    private GameObject RocketPool;
    private int RocketPoolSize = 60;
    [SerializeField]private GameObject RocketPrefab;
    private Queue<GameObject> _rocketPool;
    
    private GameObject PlasmaPool;
    private int PlasmaPoolSize = 60;
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
    //private constructor is called before static constructor
    private ObjectPooler()
    {
    }

    public void Initialize()
    {
        if (_isPoolPopulated) return;

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

        DebrisPrefab = (GameObject)Resources.Load("prefabs/PS Debris");
        DebrisPool = new GameObject("Debris Pool");

        BadGuyArrowheadPrefab = (GameObject)Resources.Load("prefabs/Bad Guy Template (Arrowhead)");
        BadGuyArrowheadPool = new GameObject("Bad Guy - Arrowhead Pool");

        _rocketPool = new Queue<GameObject>();
        _plasmaPool = new List<GameObject>();
        _laserPool = new List<GameObject>();
        _explosionLargePool = new Queue<GameObject>();
        _explosionSmallPool = new Queue<GameObject>();
        _debrisPool = new Queue<GameObject>();
        _badGuyArrowheadPool = new List<GameObject>();

        RocketPool.transform.SetParent(gameObject.transform, true);
        PlasmaPool.transform.SetParent(gameObject.transform, true);
        LaserPool.transform.SetParent(gameObject.transform, true);
        ExplosionLargePool.transform.SetParent(gameObject.transform, true);
        ExplosionSmallPool.transform.SetParent(gameObject.transform, true);
        DebrisPool.transform.SetParent(gameObject.transform, true);
        BadGuyArrowheadPool.transform.SetParent(gameObject.transform, true);

        PopulatePools();
        _isPoolPopulated = true;
    }

    private void PopulatePools()
    {
        for (var i = 0; i < RocketPoolSize; i++)
        {
            var r = Instantiate(RocketPrefab);
            r.SetActive(false);
            r.transform.SetParent(RocketPool.transform, true);
            _rocketPool.Enqueue(r);
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

        for (var i = 0; i < (ExplosionLargePoolSize / 2); i++)
        {
            var r = CreateExplosionLarge();
            _explosionLargePool.Enqueue(r);
        }

        for (var i = 0; i < (ExplosionSmallPoolSize / 2); i++)
        {
            var r = CreateExplosionSmall();
            _explosionSmallPool.Enqueue(r);
        }

        for (var i = 0; i < DebrisPoolSize; i++)
        {
            var r = CreateDebris();
            _debrisPool.Enqueue(r);
        }
    }

    private GameObject CreateExplosionSmall()
    {
        var r = Instantiate(ExplosionSmallPrefab);
        r.SetActive(false);
        r.transform.SetParent(ExplosionSmallPool.transform, true);
        ExplosionSmallAlive += 1;
        return r;
    }

    private GameObject CreateExplosionLarge()
    {
        var r = Instantiate(ExplosionLargePrefab);
        r.SetActive(false);
        r.transform.SetParent(ExplosionLargePool.transform, true);
        ExplosionLargeAlive += 1;
        return r;
    }

    private GameObject CreateDebris()
    {
        var r = Instantiate(DebrisPrefab);
        r.SetActive(false);
        r.transform.SetParent(DebrisPool.transform, true);
        _debrisPool.Enqueue(r);
        DebrisAlive += 1;
        return r;
    }

    public void PopulateBadGuyArrowheadPool(int poolSize)
    {
        if (_isBadGuyArrowheadPoolPopulated)
        {
            foreach (var o in _badGuyArrowheadPool)
            {
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
        _isBadGuyArrowheadPoolPopulated = true;
    }

    public void Reset()
    {
        if (_badGuyArrowheadPool.Count > 0)
        {
            var allBadGuys = FindObjectsOfType<BadGuyMovement>();
            foreach (var o in allBadGuys)
            {
                ResetBadGuyArrowhead(o.gameObject);
                if (!_badGuyArrowheadPool.Contains(o.gameObject))
                {
                    _badGuyArrowheadPool.Add(o.gameObject);
                }
            }
        }

        var allRockets = FindObjectsOfType<RocketFire>();
        foreach (var rocket in allRockets)
        {
            ResetRocket(rocket.gameObject);
            if (!_rocketPool.Contains(rocket.gameObject))
            {
                _rocketPool.Enqueue(rocket.gameObject);
            }
        }

        var allPlasmaBlasts = FindObjectsOfType<PlasmaFire>();
        foreach (var blast in allPlasmaBlasts)
        {
            ResetPlasma(blast.gameObject);
            if (!_plasmaPool.Contains(blast.gameObject))
            {
                _plasmaPool.Add(blast.gameObject);
            }
        }

        var allLaserBlasts = FindObjectsOfType<LaserFire>();
        foreach (var blast in allLaserBlasts)
        {
            ResetLaser(blast.gameObject);
            if (!_laserPool.Contains(blast.gameObject))
            {
                _laserPool.Add(blast.gameObject);
            }
        }
    }

    public GameObject GetRocket()
    {
        if (_rocketPool.Count == 0) return null;

        var index = _rocketPool.Count - 1;
        var r = _rocketPool.Dequeue();
        return r;
    }

    public void ReturnRocket(GameObject rocket)
    {
        ResetRocket(rocket);
        _rocketPool.Enqueue(rocket);
    }

    private void ResetRocket(GameObject rocket)
    {
        rocket.SetActive(false);
        rocket.GetComponent<RocketFire>().StopRocket();

        var rb = rocket.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rocket.transform.position = Vector3.zero;

        gameObject.GetComponentInChildren<TrailRenderer>()?.Clear();
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
        ResetPlasma(plasma);
        _plasmaPool.Add(plasma);
    }

    private void ResetPlasma(GameObject plasma)
    {
        plasma.SetActive(false);
        var rb = plasma.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(-transform.right * 3000);

        plasma.transform.position = Vector3.zero;
        plasma.transform.rotation = Quaternion.identity;
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
        ResetLaser(laser);
        _laserPool.Add(laser);
    }

    private static void ResetLaser(GameObject laser)
    {
        laser.SetActive(false);
        laser.transform.position = Vector3.zero;
    }

    public GameObject GetExplosionLarge()
    {
        if (_explosionLargePool.Count != 0) return _explosionLargePool.Dequeue();
        return ExplosionLargeAlive >= ExplosionLargePoolSize ? null : CreateExplosionLarge();
    }

    public void ReturnExplosionLarge(GameObject explosion)
    {
        explosion.SetActive(false);
        _explosionLargePool.Enqueue(explosion);
    }

    public GameObject GetExplosionSmall()
    {
        if (_explosionSmallPool.Count != 0) return _explosionSmallPool.Dequeue();
        return ExplosionSmallAlive >= ExplosionSmallPoolSize ? null : CreateExplosionSmall();
    }

    public void ReturnExplosionSmall(GameObject explosion)
    {
        explosion.SetActive(false);
        _explosionSmallPool.Enqueue(explosion);
    }

    public GameObject GetDebris()
    {
        if (_debrisPool.Count != 0) return _debrisPool.Dequeue();
        return DebrisAlive >= DebrisPoolSize ? null : CreateDebris();
    }

    public void ReturnDebris(GameObject debris)
    {
        debris.SetActive(false);
        _debrisPool.Enqueue(debris);
    }

    public void ReturnExplosionToPool(GameObject item)
    {
        item.SetActive(false);
        if (item.name.StartsWith("Explosion Small"))
        {
            _explosionSmallPool.Enqueue(item);
        }
        else if (item.name.StartsWith("Explosion Large"))
        {
            _explosionLargePool.Enqueue(item);
        }
        else if (item.name.StartsWith("PS Debris"))
        {
            _debrisPool.Enqueue(item);
        }
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
        ResetBadGuyArrowhead(o);
        _badGuyArrowheadPool.Add(o);
    }

    private static void ResetBadGuyArrowhead(GameObject o)
    {
        var bgm = o.GetComponent<BadGuyMovement>();
        bgm.Reset();

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
    }
}
