using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public GameObject levelMenuPanel;
    public GameObject leaderBoardPanel;
    public GameObject levelButton;
    public GameObject leaderBoardScoreText;
    public bool isPaused = false;

    [SerializeField]
    private GameObject mainMenuPanel;

    private void Awake()
    {
        ObjectPooler.Instance.PopulatePools();
        Explosions.Instance.Initialize();
        SoundEffectsManager.Instance.Initialize();
        EventAggregator.BaseDestroyed += EventAggregator_BaseDestroyed;
        mainMenuPanel = Resources.FindObjectsOfTypeAll<VerticalLayoutGroup>()[0].gameObject;
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.AddComponent<CameraShake>();
    }

    public void OnDestroy()
    {
        EventAggregator.BaseDestroyed -= EventAggregator_BaseDestroyed;
    }

    private void EventAggregator_BaseDestroyed(object sender, System.EventArgs e)
    {
        StateManager.isWaitingForNextLevelToStart = true;
        ShowGameOverMenu();
        //StartCoroutine(WaitForTime.Wait(1.0f, ShowGameOverMenu));
    }

    private async void ShowGameOverMenu()
    {
        await LeaderboardManager.AsyncSubmitScore(ScoreBucket.Score);

        Time.timeScale = 0.08f;

        var fireControl = GameObject.Find("FireControl");
        if (fireControl)
        {
            fireControl.GetComponent<FireControl>().StopAllBadGuyMovement();
        }

        //call UI script to replay
        if (mainMenuPanel)
        {
            mainMenuPanel.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            StartNewGame();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKey(KeyCode.P))
        {
            Pause();
        }

        if (isPaused) return;

        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 0.20f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObjectEx.IsDebug = !GameObjectEx.IsDebug;
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartNewGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ChooseLevel()
    {
        if (levelMenuPanel)
        {
            var buttonListPanel = levelMenuPanel.transform.Find("Image_Mask/Image_ButtonList");

            //build a button for each scene...
            for (var i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var button = (GameObject)Instantiate(levelButton, buttonListPanel, false);
                button.transform.Find("Text").GetComponent<Text>().text = i.ToString();

                var buttonScript = button.GetComponent<LevelButtonLaunchScene>();
                buttonScript.sceneIndex = i;
                buttonScript.panelLevels = levelMenuPanel;
            }

            levelMenuPanel.SetActive(true);
        }
    }

    public void ShowLeaderBoard()
    {
        var scoreListPanel = leaderBoardPanel.transform.Find("Image_Mask/Image_NameAndScore");

        //build a text control for each score
        var leaderBoardListCount = 10;
        for (var i = 1; i < leaderBoardListCount; i++)
        {
            var button = (GameObject)Instantiate(leaderBoardScoreText, scoreListPanel, false);
            button.transform.Find("RankText").GetComponent<Text>().text = $"{i}.";
            button.transform.Find("NameText").GetComponent<Text>().text = $"JOE";
            button.transform.Find("ScoreText").GetComponent<Text>().text = $"{i * 142}";

        }

        leaderBoardPanel.SetActive(true);
    }


    public void Restart()
    {
        StateManager.isWaitingForNextLevelToStart = true;
        ObjectPooler.Instance.Reset();
        ScoreBucket.RestoreCheckPoint();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            MusicManager.Instance.Pause();
        }
        else
        {
            Time.timeScale = 1.0f;
            MusicManager.Instance.UnPause();
        }
    }

    public void Home()
    {
        var fireControl = GameObject.Find("FireControl");
        if (fireControl)
        {
            fireControl.GetComponent<FireControl>().StopAllBadGuyMovement();
        }

        StateManager.isWaitingForNextLevelToStart = true;
        PowerUpManager.Instance.ResetPowerUp();
        ObjectPooler.Instance.Reset();
        SceneManager.LoadSceneAsync(0);
    }
}
