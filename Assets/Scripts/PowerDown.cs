using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerDownManager : MonoBehaviour
{
    public static readonly PowerDownManager Instance = (new GameObject("PowerDownSingletonContainer")).AddComponent<PowerDownManager>();

    public enum PowerDownNames
    {
        None,
        SlowMovement,
        SlowBullets,
        WeakBullets
    }

    private const float DEFAULT_ROCKET_FORCE_MULTIPLIER = 1.0f;
    private const float POWERDOWN_TIME_IN_SECONDS = 10.0f;

    public bool IsPowerDown { get; private set; }

    private int damageIncrease = 0;

    private float rocketForceMultiplier = DEFAULT_ROCKET_FORCE_MULTIPLIER;

    private bool isSlowMovement = false;
    private bool isSlowBullets = false;
    private bool isWeakBullets = false;

    private float timePowerDownExpires = 0.0f;

    private GameObject LaserBlastPrefab = null;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static PowerDownManager()
    {
    }

    private PowerDownManager()
    {
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ResetPowerDown();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        LaserBlastPrefab = (GameObject)Resources.Load("prefabs/Laser Blast");
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetPowerDown();
    }

    private void ActivatePowerDown()
    {
        var r = Random.Range(0.0f, 100.0f);
        switch (r)
        {
            case var i when i < 30.0f:
                EnableWeakBullets();
                break;
        }

        timePowerDownExpires = POWERDOWN_TIME_IN_SECONDS;
    }

    private void EnableWeakBullets()
    {
        if (!IsPowerDown)
        {
            IsPowerDown = true;
            EventAggregator.PublishPowerDownTriggered(new PowerDownTriggeredEventArgs(PowerDownNames.WeakBullets));
        }
        damageIncrease = 4;
        rocketForceMultiplier = 3.0f;
        isWeakBullets = true;
    }
    

    private void HandleWeakBullets(RocketSpawn rocketSpawn)
    {
        LaunchRocket.Instance.Launch(rocketSpawn, rocketForceMultiplier);
    }

    //private void HandleTripleBlaster(RocketSpawn rocketSpawn)
    //{
    //    //1st rocket is in normal position
    //    var rocketInstance1 = LaunchRocket.Instance.Launch(rocketSpawn, rocketForceMultiplier);

    //    if (!rocketInstance1) return;

    //    ////2nd rocket on top
    //    var newPosition = rocketInstance1.transform.TransformPoint(0, 0.8f, 0);
    //    LaunchRocket.Instance.Launch(rocketSpawn, newPosition, rocketForceMultiplier);

    //    ////3rd rocket under
    //    newPosition = rocketInstance1.transform.TransformPoint(0, -0.8f, 0);
    //    LaunchRocket.Instance.Launch(rocketSpawn, newPosition, rocketForceMultiplier);
    //}

    //private void HandleMultiBlaster()
    //{
    //    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    var allGuys = GameObject.FindObjectsOfType<GoodGuyManager>();
    //    foreach (var guy in allGuys)
    //    {
    //        var rocketSpawn = guy.AimAtMouse(mousePos);

    //        if (isSuperBlaster)
    //        {
    //            HandleTripleBlaster(rocketSpawn);
    //        }
    //        else
    //        {
    //            LaunchRocket.Instance.Launch(rocketSpawn, rocketForceMultiplier);
    //        }
    //    }
    //}

    //private void HandleLaserBlaster(RocketSpawn rocketSpawn)
    //{
    //    var l = ObjectPooler.Instance.GetLaser();
    //    if (!l) return;

    //    l.transform.position = rocketSpawn.spawnPoint.position;
    //    l.transform.rotation = rocketSpawn.spawnPoint.rotation;
    //    l.SetActive(true);

    //    const int force = 8000;

    //    var r = l.GetComponent<Rigidbody2D>();
    //    if (r)
    //    {
    //        r.AddForce(l.transform.right * force);
    //    }
    //}

    public void Update()
    {
        if (!IsPowerDown) return;

        if (timePowerDownExpires > 0)
        {
            timePowerDownExpires -= Time.deltaTime;
            if (timePowerDownExpires <= 0)
            {
                ResetPowerDown();
            }
        }
    }

    public void PowerDownHit()
    {
        if (IsPowerDown)
            return;
        ActivatePowerDown();
    }

    public void PowerDownWeakBulletsForEntireLevel()
    {
        EnableWeakBullets();
    }

    
    public void ResetPowerDown()
    {
        IsPowerDown = false;
        timePowerDownExpires = 0.0f;
        
        damageIncrease = 0;
        rocketForceMultiplier = DEFAULT_ROCKET_FORCE_MULTIPLIER;
        
        isWeakBullets = false;
        EventAggregator.PublishPowerDownExpired();
    }

    public int GetAdjustedDamage()
    {
        return damageIncrease;
    }

    public void HandlePowerDown(RocketSpawn rocketSpawn)
    {
        if (isWeakBullets)
        {
            HandleWeakBullets(rocketSpawn);
        }
        else
        {
            if (isSlowBullets)
            {
            }
            else
            {
                if (isSlowMovement)
                {
                }
            }
        }
    }
}