using System.Collections.Generic;
using UnityEngine;

public class LevelBehavior24 : MonoBehaviour
{
    public List<GameObject> WallAnimation;
    public List<GameObject> WaveSpawnPoint;
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
    private WallCloseTrigger[] _wallCloseTriggers;
    private bool[] _isDoorTriggerSet;

    void Start()
    {

        _waveDelayTime = Time.time; //needed for menu Reset
        _nextSpawnTime = Time.time;

        var fc = FindObjectOfType<FireControl>();
        _badGuyList = fc._badGuys = new Transform[BadGuysPerWave];

        var wallCloseTriggers = GameObject.FindGameObjectsWithTag("WallCloseTrigger");
        _isDoorTriggerSet = new bool[wallCloseTriggers.Length];
        _wallCloseTriggers = new WallCloseTrigger[wallCloseTriggers.Length];

        var i = 0;
        foreach (var obj in wallCloseTriggers)
        {
            _wallCloseTriggers[i++] = obj.GetComponent<WallCloseTrigger>();
        }

        StartCoroutine(WaitForTime.Wait(DoorDelayOpenTime, StartWallAnimation));

        ObjectPooler.Instance.PopulateBadGuyArrowheadPool(BadGuysPerWave);
        
        EventAggregator.BadGuyDied += EventAggregator_BadGuyDied;
        EventAggregator.WallCloseTriggered += EventAggregator_WallCloseTriggered;
    }

    private void EventAggregator_WallCloseTriggered(object sender, WallCloseTriggeredEventArgs e)
    {
        StartWallCloseAnimation();
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
        FixWallCloseStateIfNecessary();

        _deadBadGuyCount = 0;
        for (var i = 0; i < WaveSpawnPoint.Count; i++)
        {
            _isDoorTriggerSet[i] = false;
        }

        for (int i = 0; i < WaveSpawnPoint.Count; i++)
        {
            _wallCloseTriggers[i].CloseActionTriggerCollider = null;
        }

        Debug.Log($"WAVE {_currentWave} COMPLETED!");//TODO event to show big flashy message?
        if (_currentWave < HowManyWaves)
        {
            Debug.Log("Next Wave!!");//TODO event to show big flashy message?
            _currentWave++;
            badGuyIndex = 0;
            _waveDelayTime = Time.time + 0.0f; //buffer delay regardless of wave delay variable setting
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
        foreach (var animation in WallAnimation)
        {
            var animator = animation.GetComponent<Animator>();
            animator.SetTrigger("Open");
        }
    }

    public void StartWallCloseAnimation()
    {
        foreach (var animation in WallAnimation)
        {
            var animator = animation.GetComponent<Animator>();
            animator.Play("Wall Close");
            animator.SetTrigger("Close");
        }
    }

    private void FixWallCloseStateIfNecessary()
    {
        foreach (var animation in WallAnimation)
        {
            var animator = animation.GetComponent<Animator>();
            var state = animator.GetCurrentAnimatorStateInfo(0);
            animator = null;
            
        }
    }

    protected void Update()
    {
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

                foreach (var spawnPoint in WaveSpawnPoint)
                {
                    spawnPoint.DrawCircle();
                    var bg = SpawnBadGuy(spawnPoint.transform);
                    if (bg == null) return;
                    _badGuyList[badGuyIndex++] = bg;
                }
            }

            //just need one trigger even though there are two in the level
            if (badGuyIndex >= BadGuysPerWave && !_isDoorTriggerSet[0])
            {
                var lastGuyOut = _badGuyList[badGuyIndex - 1];
                _wallCloseTriggers[0].CloseActionTriggerCollider = lastGuyOut.gameObject.GetComponent<BadGuyMovement>().badGuy.GetComponent<CircleCollider2D>();
                _wallCloseTriggers[1].CloseActionTriggerCollider = _wallCloseTriggers[0].CloseActionTriggerCollider;
                _isDoorTriggerSet[0] = true;
                _isDoorTriggerSet[1] = true;
            }
        }
    }

    private Transform SpawnBadGuy(Transform spawnPoint)
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
        t.position = spawnPoint.transform.position;
        t.rotation = spawnPoint.transform.rotation;

        var bgHitConfig = movementConfig.badGuy.GetComponent<ObjectHit>();
        bgHitConfig.health = Health;

        badGuy.SetActive(true);

        return badGuy.transform;
    }

    private void OnDestroy()
    {
        EventAggregator.BadGuyDied -= EventAggregator_BadGuyDied;
        EventAggregator.WallCloseTriggered -= EventAggregator_WallCloseTriggered;
    }
}
