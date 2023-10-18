using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    public static readonly PowerUpManager Instance = (new GameObject("PowerUpSingletonContainer")).AddComponent<PowerUpManager>();

    public enum PowerUpNames
    {
        None,
        SpeedBlaster,
        TripleBlaster,
        MultiBlaster,
        LaserBlaster,
        BulletTime,
        SuperBlaster
    }

    private const float DEFAULT_ROCKET_FORCE_MULTIPLIER = 1.0f;
    private const float POWERUP_TIME_IN_SECONDS = 15.0f;

    public bool IsPowerUp { get; private set; }
    public PowerUpNames PowerUpType { get; private set; }

    private int damageIncrease = 0;

    private float rocketForceMultiplier = DEFAULT_ROCKET_FORCE_MULTIPLIER;

    private bool isSuperBlaster = false;
    private bool isMultiBlaster = false;
    private bool isTripleBlaster = false;
    private bool isLaserBlaster = false;
    private bool isSpeedBlaster = false;
    private bool isBulletTime = false;

    private float timePowerUpExpires = 0.0f;

    private GameObject LaserBlastPrefab = null;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static PowerUpManager()
    {
    }

    private PowerUpManager()
    {
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //ResetPowerUp();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        //LaserBlastPrefab = (GameObject)Resources.Load("prefabs/Laser Blast");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    public void ForcePowerUp(PowerUpManager.PowerUpNames forcePowerUp)
    {
        switch (forcePowerUp)
        {
            case PowerUpNames.SpeedBlaster:
                PowerUpSpeedBlasterForEntireLevel();
                break;
            case PowerUpNames.MultiBlaster:
                PowerUpMultiBlasterForEntireLevel();
                break;
            case PowerUpNames.TripleBlaster:
                PowerUpTripleBlasterForEntireLevel();
                break;
            case PowerUpNames.BulletTime:
                PowerUpBulletTimeForEntireLevel();
                break;
            case PowerUpNames.LaserBlaster:
                PowerUpLaserBlasterForEntireLevel();
                break;
            case PowerUpNames.SuperBlaster:
                PowerUpSuperBlasterForEntireLevel();
                break;
        }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetPowerUp();
    }

    private void ActivatePowerUp()
    {
        var r = Random.Range(0.0f, 100.0f);
        switch (r)
        {
            case var i when i < 30.0f:
                EnableSpeedBlaster();
                break;
            case var i when i >= 30.0f && i < 60.0f:
                EnableTripleBlaster();
                break;
            case var i when i >= 60.0f && i < 80.0f:
                EnableMultiBlaster();
                break;
            case var i when i >= 80.0f && i < 90.0f:
                //EnableLaserBlaster();//laser blaster not ready
                EnableisBulletTime();
                break;
            case var i when i >= 90.0f:
                EnableSuperBlaster();
                break;
        }

        timePowerUpExpires = POWERUP_TIME_IN_SECONDS;
    }

    private void EnableSpeedBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            EventAggregator.PublishPowerUpTriggered(new PowerUpTriggeredEventArgs(PowerUpNames.SpeedBlaster));
        }
        damageIncrease = 6;
        rocketForceMultiplier = 3.0f;
        isSpeedBlaster = true;
        PowerUpType = PowerUpNames.SpeedBlaster;
    }

    private void EnableTripleBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            EventAggregator.PublishPowerUpTriggered(new PowerUpTriggeredEventArgs(PowerUpNames.TripleBlaster));
        }
        damageIncrease = 3;
        isTripleBlaster = true;
        PowerUpType = PowerUpNames.TripleBlaster;
    }

    private void EnableMultiBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            EventAggregator.PublishPowerUpTriggered(new PowerUpTriggeredEventArgs(PowerUpNames.MultiBlaster));
        }
        damageIncrease = 3;
        isMultiBlaster = true;
        PowerUpType = PowerUpNames.MultiBlaster;
    }

    private void EnableLaserBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            EventAggregator.PublishPowerUpTriggered(new PowerUpTriggeredEventArgs(PowerUpNames.LaserBlaster));
        }
        damageIncrease = 10;
        isLaserBlaster = true;
        PowerUpType = PowerUpNames.LaserBlaster;
    }

    private void EnableisBulletTime()
    {
        IsPowerUp = false;

        //if (!IsPowerUp)
        //{
        //    IsPowerUp = true;
        //    //EventAggregator.PublishPowerUpTriggered(new PowerUpTriggeredEventArgs(PowerUpNames.BulletTime));
        //}
        //isBulletTime = true;
        //PowerUpType = PowerUpNames.BulletTime;

        Time.timeScale = 0.20f;

        StartCoroutine(ReturnAfterTimeOut());

        //StartCoroutine(WaitForTime.Wait(1.0f, () =>
        //{
        //    Time.timeScale = 1f;
        //}));

    }

    private float Timeout = 2.0f;
    private IEnumerator ReturnAfterTimeOut()
    {
        var elapsed = 0.0f;
        while (elapsed < Timeout)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
    }

    private void EnableSuperBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            EventAggregator.PublishPowerUpTriggered(new PowerUpTriggeredEventArgs(PowerUpNames.SuperBlaster));
        }
        isSuperBlaster = true;
        EnableSpeedBlaster();
        EnableTripleBlaster();
        EnableMultiBlaster();
        EnableLaserBlaster();
        PowerUpType = PowerUpNames.SuperBlaster;
        damageIncrease = 2;//balance power back out
    }

    private void HandleSpeedBlaster(RocketSpawn rocketSpawn)
    {
        LaunchRocket.Launch(rocketSpawn, rocketForceMultiplier);
    }

    private void HandleTripleBlaster(RocketSpawn rocketSpawn)
    {
        //1st rocket is in normal position
        var rocketInstance1 = LaunchRocket.Launch(rocketSpawn, rocketForceMultiplier);

        if (!rocketInstance1) return;

        ////2nd rocket on top
        var newPosition = rocketInstance1.transform.TransformPoint(0, 0.8f, 0);
        LaunchRocket.Launch(rocketSpawn, newPosition, rocketForceMultiplier);

        ////3rd rocket under
        newPosition = rocketInstance1.transform.TransformPoint(0, -0.8f, 0);
        LaunchRocket.Launch(rocketSpawn, newPosition, rocketForceMultiplier);
    }

    private void HandleMultiBlaster()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var allGuys = GameObject.FindObjectsOfType<GoodGuyManager>();
        foreach (var guy in allGuys)
        {
            var rocketSpawn = guy.AimAtMouse(mousePos);

            if (isSuperBlaster)
            {
                HandleTripleBlaster(rocketSpawn);
            }
            else
            {
                LaunchRocket.Launch(rocketSpawn, rocketForceMultiplier);
            }
        }
    }

    private void HandleLaserBlaster(RocketSpawn rocketSpawn)
    {
        var l = ObjectPooler.Instance.GetLaser();
        if (!l) return;

        l.transform.position = rocketSpawn.spawnPoint.position;
        l.transform.rotation = rocketSpawn.spawnPoint.rotation;
        l.SetActive(true);

        const int force = 12 * 1000;

        var r = l.GetComponent<Rigidbody2D>();
        if (r)
        {
            r.AddForce(l.transform.right * force);
        }
    }

    private void HandleSuperBlaster()
    {
        HandleMultiBlaster();
    }

    public void Update()
    {
        if (!IsPowerUp) return;

        if (timePowerUpExpires > 0)
        {
            timePowerUpExpires -= Time.deltaTime;
            if (timePowerUpExpires <= 0)
            {
                ResetPowerUp();
            }
        }
    }

    public void PowerUpHit()
    {
        if (IsPowerUp)
            return;
        ActivatePowerUp();
    }

    public void PowerUpSpeedBlasterForEntireLevel()
    {
        EnableSpeedBlaster();
    }

    public void PowerUpMultiBlasterForEntireLevel()
    {
        EnableMultiBlaster();
    }

    public void PowerUpTripleBlasterForEntireLevel()
    {
        EnableTripleBlaster();
    }

    public void PowerUpLaserBlasterForEntireLevel()
    {
        EnableLaserBlaster();
    }
    public void PowerUpBulletTimeForEntireLevel()
    {
        EnableisBulletTime();
    }

    public void PowerUpSuperBlasterForEntireLevel()
    {
        EnableSuperBlaster();
    }

    public void ResetPowerUp()
    {
        IsPowerUp = false;
        PowerUpType = PowerUpNames.None;
        timePowerUpExpires = 0.0f;
        damageIncrease = 0;
        rocketForceMultiplier = DEFAULT_ROCKET_FORCE_MULTIPLIER;
        isSuperBlaster = false;
        isMultiBlaster = false;
        isSpeedBlaster = false;
        isTripleBlaster = false;
        isLaserBlaster = false;
        EventAggregator.PublishPowerUpExpired();
    }

    public int GetAdjustedDamage()
    {
        return damageIncrease;
    }

    public void HandlePowerUp(RocketSpawn rocketSpawn)
    {
        if (isSuperBlaster)
        {
            HandleSuperBlaster();
        }
        else
        {
            if (isTripleBlaster)
            {
                HandleTripleBlaster(rocketSpawn);
            }
            else
            {
                if (isMultiBlaster)
                {
                    HandleMultiBlaster();
                }
                else
                {
                    if (isLaserBlaster)
                    {
                        HandleLaserBlaster(rocketSpawn);
                    }
                    else
                    {
                        if (isSpeedBlaster)
                        {
                            HandleSpeedBlaster(rocketSpawn);
                        }
                    }
                }
            }
        }
    }
}