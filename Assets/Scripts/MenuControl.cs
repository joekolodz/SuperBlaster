using Assets.Scripts;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private TMP_InputField playerNameInputField;

    private int currentSceneIndex = 0;
    private GameObject quickMenuPanel;

    private void Awake()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            GetPlayerName();
        }
        SoundEffectsManager.Instance.Initialize();
        ObjectPooler.Instance.Initialize();
        Siren.Instance.Initialize();
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
        //TODO LeaderboardManager
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
            quickMenuPanel = GameObject.Find("QuickMenu");
            quickMenuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (currentSceneIndex == 0) return;

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

        if(Input.GetKeyUp(KeyCode.Space))
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
        StartLevel(1);
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

    private readonly List<GameObject> _leaderboardPlayerList = new List<GameObject>();
    public async void ShowLeaderBoard()
    {
        //TODO LeaderboardManager
        //return;

        if (leaderBoardPanel.activeInHierarchy) return;
        leaderBoardPanel.SetActive(true);

        var scoreListPanel = leaderBoardPanel.transform.Find("Image_Mask/Image_NameAndScore");
        var pleaseWaitText = leaderBoardPanel.transform.Find("PleaseWait").gameObject;

        pleaseWaitText.SetActive(true);
        
        var board = await LeaderboardManager.AsyncGetLeaderboard();

        pleaseWaitText.SetActive(false);
        
        Debug.Log($"LeaderBoardManager: {board.Length}");

        foreach (var player in board)
        {
            var button = (GameObject)Instantiate(leaderBoardScoreText, scoreListPanel, false);
            button.transform.Find("RankText").GetComponent<Text>().text = $"{player.rank}.";
            button.transform.Find("NameText").GetComponent<Text>().text = $"{player.player.name}";
            button.transform.Find("ScoreText").GetComponent<Text>().text = $"{player.score}";
            _leaderboardPlayerList.Add(button);
        }
    }

    public void GetPlayerName()
    {
        var s = PlayerPrefs.GetString("PlayerName");
        playerNameInputField.text = s;
    }

    public async void ChangePlayerName()
    {
        var s = playerNameInputField.text;
        PlayerPrefs.SetString("PlayerName", s);
        PlayerPrefs.Save();
        await LeaderboardManager.AsyncChangePlayerName(s);
    }

    public void HideLeaderBoard()
    {
        leaderBoardPanel.SetActive(false);
        _leaderboardPlayerList.ForEach(Destroy);
        _leaderboardPlayerList.Clear();
    }

    public void Restart()
    {
        ScoreBucket.RestoreCheckPoint();
        StartLevel(SceneManager.GetActiveScene().buildIndex);
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
        EventAggregator.PublishAbortLevel();

        var fireControl = GameObject.Find("FireControl");
        if (fireControl)
        {
            fireControl.GetComponent<FireControl>().StopAllBadGuyMovement();
        }

        StartLevel(0);
    }

    private void StartLevel(int sceneIndex)
    {
        StateManager.isWaitingForNextLevelToStart = true;
        ObjectPooler.Instance.Reset();
        PowerUpManager.Instance.ResetPowerUp();
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
