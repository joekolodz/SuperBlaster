﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    /// <summary>
    /// Add a new AudioSource for each music track, then add it to this list
    /// </summary>
    public System.Collections.Generic.List<AudioSource> PlayList;

    private AudioSource activeMusic;
    private bool isMainMenuMusicPlaying = false;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //Returning to the main menu will attempt to re-create a new music manager. do not allow that.
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            DontDestroyOnLoad(gameObject);
            SceneManager_activeSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
        }
    }

    private void SceneManager_activeSceneChanged(Scene current, Scene next)
    {
        if (!string.IsNullOrWhiteSpace(next.name))
        {
            if (next.buildIndex == 0)
            {
                Stop();
                activeMusic = PlayList[0];
                Play();
                isMainMenuMusicPlaying = true;
            }
            else
            {
                if (isMainMenuMusicPlaying)
                {
                    Stop();
                    activeMusic = PlayList[1];//could be random or based on level
                    Play();
                    isMainMenuMusicPlaying = false;
                }
            }
        }
    }

    public void Play()
    {
        if (activeMusic == null) return;
        activeMusic.volume = 0.1f;
        activeMusic.Play();
    }

    public void Play(int sceneIndex)
    {
        if (activeMusic == null) return;
        activeMusic.Play();
    }

    public void Stop()
    {
        if (activeMusic == null) return;
        activeMusic.Stop();
    }

    public void Pause()
    {
        if (activeMusic == null) return;
        activeMusic.Pause();
    }

    public void UnPause()
    {
        if (activeMusic == null) return;
        activeMusic.UnPause();
    }
}
