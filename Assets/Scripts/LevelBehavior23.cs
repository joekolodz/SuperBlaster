using UnityEngine;

public class LevelBehavior23 : MonoBehaviour
{
    public GameObject WallAnimation;
    public GameObject WaveSpawnPoint;
    public float DoorDelayOpenTime = 1.5f;
    public int BadGuysPerWave = 10;
    public int HowManyWaves = 1;
    public float SpawnInterval = 0.5f;
    public float WaveDelay = 1.5f;
    public int RoundsPerBurst = 4;
    public float DelayBetweenRounds = 1.0f;
    public float DelayBetweenShots = 1.0f;
    public float Accuracy = 0.3f;
    public int MoveSpeed = 8;
    public int Health = 10;

    private float _waveDelayTime;
    private float _nextSpawnTime;
    private int _currentWave = 0;
    private Transform[] _badGuyList;
    private int badGuyIndex;

    void Start()
    {
        _waveDelayTime = Time.time; //needed for menu Reset
        _nextSpawnTime = Time.time;

        var fc = FindObjectOfType<FireControl>();
        _badGuyList = fc._badGuys = new Transform[BadGuysPerWave];

        StartCoroutine(WaitForTime.Wait(DoorDelayOpenTime, StartWallAnimation));

        ObjectPooler.Instance.PopulateBadGuyArrowheadPool(BadGuysPerWave);
        EventAggregator.BadGuyDied += EventAggregator_BadGuyDied;
        EventAggregator.AllBadGuysKilled += EventAggregator_AllBadGuysKilled;
    }

    private void EventAggregator_AllBadGuysKilled(object sender, System.EventArgs e)
    {
        //inc next wave counter
        //start next wave if there is one
    }

    private void EventAggregator_BadGuyDied(object sender, BadGuyDiedEventArgs e)
    {
        ObjectPooler.Instance.ReturnBadGuyArrowhead(e.BadGuy);
    }

    private void StartWallAnimation()
    {
        var animation = WallAnimation.GetComponent<Animator>();
        animation.SetTrigger("Open");
    }


    protected void Update()
    {
        WaveSpawnPoint.DrawCircle();

        //initial delay before spawn
        if (Time.time > _waveDelayTime + WaveDelay)
        {
            //spawner
            if (Time.time > _nextSpawnTime && badGuyIndex < BadGuysPerWave)
            {
                _nextSpawnTime = Time.time + SpawnInterval;
                var bg = SpawnBadGuy();
                if (bg == null) return;
                _badGuyList[badGuyIndex++] = bg;
            }

            //after all bad guys are dead
            //_waveDelayTime = Time.time;
        }
    }

    private Transform SpawnBadGuy()
    {
        var badGuy = ObjectPooler.Instance.GetBadGuyArrowhead();

        if (badGuy == null) return null;

        var plasmaConfig = badGuy.GetComponent<PlasmaSpawn>();
        plasmaConfig.roundsPerBurst = RoundsPerBurst;
        plasmaConfig.delayBetweenRounds = DelayBetweenRounds;
        plasmaConfig.delayBetweenShots = DelayBetweenShots;
        plasmaConfig.accuracy = Accuracy;
        plasmaConfig.isFiring = false;

        var movementConfig = badGuy.GetComponent<BadGuyMovement>();
        movementConfig.moveSpeed = MoveSpeed;
        movementConfig.IsNextWaypointRandom = true;

        var t = movementConfig.badGuy.transform;
        t.position = WaveSpawnPoint.transform.position;
        t.rotation = WaveSpawnPoint.transform.rotation;

        var bgHitConfig = movementConfig.badGuy.GetComponent<ObjectHit>();
        bgHitConfig.health = Health;

        badGuy.SetActive(true);

        return badGuy.transform;
    }

    private void OnDestroy()
    {
        EventAggregator.BadGuyDied -= EventAggregator_BadGuyDied;
        EventAggregator.AllBadGuysKilled -= EventAggregator_AllBadGuysKilled;
    }
}
