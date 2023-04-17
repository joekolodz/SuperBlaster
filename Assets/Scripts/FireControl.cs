using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FireControl : MonoBehaviour
{
    public Transform[] _goodGuys;
    public Transform[] _badGuys;
    public GameObject _getReadyText;

    private int rocketCost = 1;

    private Text scoreText;
    private Text highScoreText;

    private GoodGuyManager _currentlySelectedGuy;
    private bool isAllDestroyed = false;

    public void Awake()
    {
        Time.timeScale = 1.0f;

        isAllDestroyed = true;
        StateManager.isWaitingForNextLevelToStart = false;

        PlaceScoreText();

        ScoreBucket.LoadHighScore();
        UpdateScore();

        foreach (var guy in _goodGuys)
        {
            var manager = guy.GetComponent<GoodGuyManager>();
            if (manager.isSelectedOnStartup)
            {
                SimpleLog.Log($"AWAKE - Select Guy");
                SelectGuy(manager);
                break;
            }
        }
        EventAggregator.BadGuyDied += EventAggregator_BadGuyDied;
        EventAggregator.LevelCompleted += EventAggregator_LevelCompleted;
        EventAggregator.PowerUpTriggered += EventAggregator_PowerUpTriggered;
    }

    private void OnDestroy()
    {
        EventAggregator.BadGuyDied -= EventAggregator_BadGuyDied;
        EventAggregator.LevelCompleted -= EventAggregator_LevelCompleted;
        EventAggregator.PowerUpTriggered -= EventAggregator_PowerUpTriggered;
    }

    private void EventAggregator_PowerUpTriggered(object sender, PowerUpTriggeredEventArgs e)
    {
        AddScore(500);
    }

    private void EventAggregator_LevelCompleted(object sender, System.EventArgs e)
    {
        StartCoroutine(StartNextLevel());
    }

    private void EventAggregator_BadGuyDied(object sender, BadGuyDiedEventArgs e)
    {
        AddScore(e.DestructionValue);
    }

    public void PlaceScoreText()
    {
        var canvas = GameObject.Find("Canvas").transform;

        var resource = Resources.Load("ScoreText", typeof(Text));
        scoreText = Instantiate(resource, new Vector3(31, 28, 0), new Quaternion(), canvas) as Text;

        resource = Resources.Load("HighScoreText", typeof(Text));
        highScoreText = Instantiate(resource, new Vector3(-45, 28, 0), new Quaternion(), canvas) as Text;

        UpdateScore();
    }


    private GameObject myrocket;
    public void Update()
    {
        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        bool isFiring = Input.GetButtonDown("Fire1");

        if (isFiring)
        {
            //are we clicking on something?
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var guy = IsGuySelected(mousePos);
            if (guy)
            {
                SimpleLog.Log("clicked on a guy");

                SimpleLog.Log("UPDATE - Select Guy");
                SelectGuy(guy);
            }
            else
            {
                SimpleLog.Log($"Already have a guy: NULL={_currentlySelectedGuy is null}");
                SimpleLog.Log($"Already have a guy: isSelected={_currentlySelectedGuy?.isSelected}");
                FireARocket(_currentlySelectedGuy, mousePos);
            }
        }

        //should be an event from bad guys instead of polling
        DetectBadGuyDeathState();

    }

    private void DetectBadGuyDeathState()
    {
        foreach (var t in _badGuys)
        {
            if (t == null) return;

            var bgm = t.GetComponent<BadGuyMovement>();
            if (!bgm.isDestroyed) return;

            var badGuy = bgm.badGuy;
            if (badGuy != null)
            {
                var od = badGuy.GetComponentInChildren<ObjectDestroy>();
                if (od.IsPooledObject) return;
            }
        }

        if (!StateManager.isWaitingForNextLevelToStart)
        {
            StartCoroutine(StartNextLevel());
        }
    }

    IEnumerator StartNextLevel()
    {
        //Display GET READY and pause for next level

        StateManager.isWaitingForNextLevelToStart = true;
        StopAllBadGuyMovement();
        PowerUpManager.Instance.ResetPowerUp();
        ScoreBucket.SaveHighScore();
        Instantiate(_getReadyText, GameObject.Find("Canvas").transform, false);
        yield return new WaitForSeconds(4);

        var sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
            //increase difficulty?
        }
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void StopAllBadGuyMovement()
    {
        foreach (var badGuy in _badGuys)
        {
            if (badGuy)
            {
                badGuy.GetComponent<BadGuyMovement>().moveSpeed = 0;
                badGuy.GetComponent<PlasmaSpawn>().roundsPerBurst = 0;
            }
        }
    }

    private GoodGuyManager IsGuySelected(Vector3 mousePos)
    {
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);//use V2 because we don't want the Z axis
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); //zero means straight down under the mousepoint

        if (hit.collider != null)
        {
            //can we find the selected guy?
            return hit.collider.gameObject.GetComponent<GoodGuyManager>();
        }
        return null;
    }

    void FireARocket(GoodGuyManager guyFiring, Vector3 mousePos)
    {
        SimpleLog.Log("Firing a rocket");

        SimpleLog.Log("isWaitingForNextLevelToStart");
        if (StateManager.isWaitingForNextLevelToStart) return;

        SimpleLog.Log("don't fire when mouse is in menu area");

        if (mousePos.y < -24) return; //don't fire when mouse is in menu area

        SimpleLog.Log("is selected");
        if (guyFiring.isSelected)
        {
            SimpleLog.Log("Aim At Mouse");
            var rocketSpawn = guyFiring.AimAtMouse(mousePos);

            SimpleLog.Log("IsPowerUp");
            if (PowerUpManager.Instance.IsPowerUp)
            {
                SimpleLog.Log("HandlePowerUp");
                PowerUpManager.Instance.HandlePowerUp(rocketSpawn);
            }
            else
            {
                SimpleLog.Log("Instance Launch");
                LaunchRocket.Instance.Launch(rocketSpawn);
            }

            SimpleLog.Log("Buy Rocket");
            BuyRocket(rocketCost);
        }
    }

    public void UnselectAllGuys()
    {
        SimpleLog.Log("!!! Unselect All Guys !!!");
        foreach (var guy in _goodGuys)
        {
            guy.GetComponent<GoodGuyManager>().Unselect();
        }
    }

    public void SelectGuy(GoodGuyManager guy)
    {
        SimpleLog.Log("Unselect all guys");

        UnselectAllGuys();
        
        SimpleLog.Log("Select one guy");
        guy.Select();
        
        SimpleLog.Log("assign cache");
        _currentlySelectedGuy = guy;
        SimpleLog.Log($"guy: isSelected={guy?.isSelected}");
        SimpleLog.Log($"cache: isSelected={_currentlySelectedGuy?.isSelected}");

    }

    public void AddScore(int value)
    {
        ScoreBucket.AddScore(value);
        UpdateScore();
    }

    public void BuyRocket(int cost)
    {
        ScoreBucket.BuyRocket(cost);
        UpdateScore();
    }

    public void UpdateScore()
    {
        scoreText.text = "SCORE: " + ScoreBucket.Score;
        highScoreText.text = "HIGH SCORE: " + ScoreBucket.HighScore;
    }
}
