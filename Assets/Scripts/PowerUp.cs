using UnityEngine;

public class PowerUp
{
    public static readonly PowerUp Instance = new PowerUp();

    private const float DEFAULT_ROCKET_FORCE_MULTIPLIER = 1.0f;

    public bool IsPowerUp { get; private set; }

    private int damageIncrease = 0;

    private float rocketForceMultiplier = DEFAULT_ROCKET_FORCE_MULTIPLIER;

    private bool isSuperBlaster = false;
    private bool isMultiBlaster = false;
    private bool isTripleBlaster = false;
    private bool isSpeedBlaster = false;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static PowerUp()
    {
    }

    private PowerUp()
    {
    }

    public void IsPowerUpHit(string tag)
    {
        if (IsPowerUp)
            return;

        if (tag == "PowerUp")
        {
            //TODO set a timer/counter to turn off powerup?
            var r = Random.Range(0.0f, 100.0f);
            Debug.Log($"Random Number: {r}");
            switch (r)
            {
                case var i when i < 40.0f:
                    EnableSpeedBlaster();
                    break;
                case var i when i >= 40.0f && i < 70.0f:
                    EnableTripleBlaster();
                    break;
                case var i when i >= 70.0f && i < 90.0f:
                    EnableMultiBlaster();
                    break;
                case var i when i >= 90.0f:
                    EnableSuperBlaster();
                    break;
            }
        }
    }

    public void ResetPowerUp()
    {
        IsPowerUp = false;
        damageIncrease = 0;
        rocketForceMultiplier = DEFAULT_ROCKET_FORCE_MULTIPLIER;
        isSuperBlaster = false;
        isSpeedBlaster = false;
        isTripleBlaster = false;

        //change title graphic back to normal Super Blaster
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
                    if (isSpeedBlaster)
                    {
                        HandleSpeedBlaster(rocketSpawn);
                    }
                }
            }
        }
    }

    public void EnableSpeedBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            GameObject.FindObjectOfType<TitleSwap>().SetTitle(TitleSwap.TitleNames.SpeedBlaster);
        }
        damageIncrease = 2;
        rocketForceMultiplier = 2.33f;
        isSpeedBlaster = true;
    }

    public void EnableTripleBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            GameObject.FindObjectOfType<TitleSwap>().SetTitle(TitleSwap.TitleNames.TripleBlaster);
        }
        damageIncrease = 3;
        isTripleBlaster = true;
    }

    public void EnableMultiBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            GameObject.FindObjectOfType<TitleSwap>().SetTitle(TitleSwap.TitleNames.MultiBlaster);
        }
        isMultiBlaster = true;
    }

    public void EnableSuperBlaster()
    {
        if (!IsPowerUp)
        {
            IsPowerUp = true;
            GameObject.FindObjectOfType<TitleSwap>().SetTitle(TitleSwap.TitleNames.SuperBlaster);
        }
        isSuperBlaster = true;
        EnableSpeedBlaster();
        EnableTripleBlaster();
        EnableMultiBlaster();
    }

    public int GetAdjustedDamage()
    {
        return damageIncrease;
    }

    private void HandleSpeedBlaster(RocketSpawn rocketSpawn)
    {
        LaunchRocket.Instance.Launch(rocketSpawn, rocketForceMultiplier);
    }

    private void HandleTripleBlaster(RocketSpawn rocketSpawn)
    {
        //1st rocket is in normal position
        var rocketInstance1 = LaunchRocket.Instance.Launch(rocketSpawn, rocketForceMultiplier);

        ////2nd rocket on top
        var newPosition = rocketInstance1.transform.TransformPoint(0, 0.8f, 0);
        LaunchRocket.Instance.Launch(rocketSpawn, newPosition, rocketForceMultiplier);

        ////3rd rocket under
        newPosition = rocketInstance1.transform.TransformPoint(0, -0.8f, 0);
        LaunchRocket.Instance.Launch(rocketSpawn, newPosition, rocketForceMultiplier);
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
                LaunchRocket.Instance.Launch(rocketSpawn, rocketForceMultiplier);
            }
        }
    }

    private void HandleSuperBlaster()
    {
        HandleMultiBlaster();
    }
}