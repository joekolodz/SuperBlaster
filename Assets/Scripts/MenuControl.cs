﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public GameObject levelMenuPanel;
    public GameObject levelButton;
    public bool isPaused = false;

    private AudioSource _music = null;

    private void Start()
    {
        _music = GameObject.Find("Background Music").GetComponent<AudioSource>();

    }
    void Update()
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

            Debug.Log($"scenesInBuild:{SceneManager.sceneCountInBuildSettings}");

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

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
            _music.Pause();
        }
        else
        {
            Time.timeScale = 1.0f;
            _music.UnPause();
        }
    }
}
