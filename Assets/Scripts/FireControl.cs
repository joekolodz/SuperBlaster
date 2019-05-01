using System.Collections;
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

    void Start()
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
                SelectGuy(manager);
                break;
            }
        }
    }

    public void PlaceScoreText()
    {
        var canvas = GameObject.Find("Canvas").transform;

        var resource = Resources.Load("ScoreText", typeof(Text));
        scoreText = Instantiate(resource, new Vector3(31,28,0), new Quaternion(), canvas) as Text;

        resource = Resources.Load("HighScoreText", typeof(Text));
        highScoreText = Instantiate(resource, new Vector3(-45, 28, 0), new Quaternion(), canvas) as Text;

        UpdateScore();
    }

    public void Update()
    {
        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        if(Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 0.20f;
        }
        else
        {
            Time.timeScale = 1f;
        }


        bool isFiring = Input.GetButtonDown("Fire1");

        if (isFiring)
        {
            //are we clicking on something?
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var guy = IsGuySelected(mousePos);
            if (guy)
            {
                SelectGuy(guy);
            }
            else
            {
                FireARocket(_currentlySelectedGuy, mousePos);
            }
        }

        //should be an event from bad guys instead of polling
        DetectBadGuyDeathState();

    }

    private void DetectBadGuyDeathState()
    {
        isAllDestroyed = true;
        foreach (var t in _badGuys)
        {
            if (t.GetComponent<BadGuyMovement>().isDestroyed) continue;
            isAllDestroyed = false;
        }

        if (isAllDestroyed && !StateManager.isWaitingForNextLevelToStart)
        {
            StartCoroutine(StartNextLevel());
        }
    }

    IEnumerator StartNextLevel()
    {
        //Display GET READY and pause for next level

        StateManager.isWaitingForNextLevelToStart = true;
        StopAllBadGuyMovement();
        PowerUp.Instance.ResetPowerUp();
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
        if (StateManager.isWaitingForNextLevelToStart) return;

        if (mousePos.y < -24) return; //don't fire when mouse is in menu area

        if (guyFiring.isSelected)
        {
            var rocketSpawn = guyFiring.AimAtMouse(mousePos);

            if (PowerUp.Instance.IsPowerUp)
            {
                PowerUp.Instance.HandlePowerUp(rocketSpawn);
            }
            else
            {
                LaunchRocket.Instance.Launch(rocketSpawn);
            }

            BuyRocket(rocketCost);
        }
    }

    public void UnselectAllGuys()
    {
        foreach (var guy in _goodGuys)
        {
            guy.GetComponent<GoodGuyManager>().Unselect();
        }
    }

    public void SelectGuy(GoodGuyManager guy)
    {
        UnselectAllGuys();
        guy.Select();
        _currentlySelectedGuy = guy;
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
