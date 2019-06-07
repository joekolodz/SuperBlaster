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
    private int _currentWave = 1;
    private Transform[] _badGuyList;
    private int badGuyIndex;
    private WallCloseTrigger _wallCloseTrigger;
    private bool _isDoorTriggerSet = false;

    void Start()
    {
        _waveDelayTime = Time.time; //needed for menu Reset
        _nextSpawnTime = Time.time;

        var fc = FindObjectOfType<FireControl>();
        _badGuyList = fc._badGuys = new Transform[BadGuysPerWave];
        _wallCloseTrigger = GameObject.Find("WallCloseTrigger").GetComponent<WallCloseTrigger>();

        StartCoroutine(WaitForTime.Wait(DoorDelayOpenTime, StartWallAnimation));

        ObjectPooler.Instance.PopulateBadGuyArrowheadPool(BadGuysPerWave);
        EventAggregator.BadGuyDied += EventAggregator_BadGuyDied;
    }

    private int _deadBadGuyCount = 0;
    private void EventAggregator_BadGuyDied(object sender, BadGuyDiedEventArgs e)
    {
        ObjectPooler.Instance.ReturnBadGuyArrowhead(e.BadGuy);
        _deadBadGuyCount++;

        if (_deadBadGuyCount >= BadGuysPerWave)
        {
            SetupNextWave();
        }
    }

    private void SetupNextWave()
    {
        _deadBadGuyCount = 0;
        _isDoorTriggerSet = false;
        _wallCloseTrigger.CloseActionTriggerCollider = null;
        Debug.Log($"WAVE {_currentWave} COMPLETED!");//TODO event to show big flashy message?
        if (_currentWave < HowManyWaves)
        {
            Debug.Log("Next Wave!!");//TODO event to show big flashy message?
            _currentWave++;
            badGuyIndex = 0;
            _waveDelayTime = Time.time + 1.0f; //buffer delay regardless of wave delay variable setting
            _nextSpawnTime = Time.time + SpawnInterval;
            StartCoroutine(WaitForTime.Wait(DoorDelayOpenTime, StartWallAnimation));
        }
        else
        {
            EventAggregator.PublishLevelCompleted();
        }
    }

    private void StartWallAnimation()
    {
        var animator = WallAnimation.GetComponent<Animator>();
        animator.SetTrigger("Open");
    }

    public void StartWallCloseAnimation()
    {
        var animator = WallAnimation.GetComponent<Animator>();
        animator.Play("Wall Close");
        animator.SetTrigger("Close");
    }

    protected void Update()
    {
        WaveSpawnPoint.DrawCircle();
        SpawnSequence();
    }

    private void SpawnSequence()
    {
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

            if (badGuyIndex >= BadGuysPerWave && !_isDoorTriggerSet)
            {
                var lastGuyOut = _badGuyList[badGuyIndex - 1];
                _wallCloseTrigger.CloseActionTriggerCollider = lastGuyOut.gameObject.GetComponent<BadGuyMovement>().badGuy.GetComponent<CircleCollider2D>();
                _isDoorTriggerSet = true;
            }
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
    }
}
