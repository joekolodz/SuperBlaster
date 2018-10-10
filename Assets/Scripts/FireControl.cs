using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ScoreBucket
{
    public static int Score = 0;
    public static int HighScore = 0;

    public static void AddScore(int score)
    {
        Score+=score;
        UpdateHighScore();
    }

    public static void BuyRocket(int cost)
    {
        Score-=cost;
        if(Score<0)
        {
            Score = 0;
        }
        UpdateHighScore();
    }

    private static void UpdateHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
        }
    }

    public static void SaveHighScore()
    {
        PlayerPrefs.SetInt("highscore",HighScore);
        PlayerPrefs.Save();
    }

    public static void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt("highscore");
    }
}

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
    private bool isWaitingForDelay = false;

    void Start()
    {
        isAllDestroyed = true;
        isWaitingForDelay = false;

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
        var resource = Resources.Load("ScoreText", typeof(Text));
        scoreText = Instantiate(resource, GameObject.Find("Canvas").transform, false) as Text;

        resource = Resources.Load("HighScoreText", typeof(Text));
        highScoreText = Instantiate(resource, GameObject.Find("Canvas").transform, false) as Text;

        UpdateScore();
    }

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

        if (isAllDestroyed && !isWaitingForDelay)
        {
            isWaitingForDelay = true;
            StartCoroutine(StartNextLevel());
        }
    }

    IEnumerator StartNextLevel()
    {
        //Display GET READY and pause for next level
        ScoreBucket.SaveHighScore();
        Instantiate(_getReadyText, GameObject.Find("Canvas").transform, false);
        yield return new WaitForSeconds(4);

        var sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex > 13)
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
            badGuy.GetComponent<BadGuyMovement>().moveSpeed = 0;
            badGuy.GetComponent<PlasmaSpawn>().roundsPerBurst = 0;
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
        if (mousePos.y < -24) return; //don't fire when mouse is in menu area

        if (guyFiring.isSelected)
        {
            //need to get the angle from the rocket launcher since it is above center of the guy (which would cause a bad angle if we used guy's position)
            //var rocketLauncher2 = gameObject.GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(t => t.name == "Rocket Launcher");
            var rocketLauncher = guyFiring.transform.Find("Good Guy/Rocket Launcher");


            //first, turn guy to face mouse click point (using rocket position instead of guy position)
            if (guyFiring.guy.rotation.eulerAngles.y == 180)
            {
                //call reverse since the guys on the right are flipped 180
                guyFiring.transform.rotation = GeometricFunctions.RotateToFace(mousePos, rocketLauncher.transform.position);
            }
            else
            {
                guyFiring.transform.rotation = GeometricFunctions.RotateToFace(rocketLauncher.transform.position, mousePos);
            }

            var rocketSpawn = guyFiring.GetComponent<RocketSpawn>();
            //then instantiate rocket
            Instantiate(rocketSpawn.rocket, rocketSpawn.spawnPoint.position, rocketSpawn.spawnPoint.rotation);
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
