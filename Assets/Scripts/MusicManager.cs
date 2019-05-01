using UnityEngine;
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
        activeMusic?.Play();
    }

    public void Play(int sceneIndex)
    {
        activeMusic?.Play();
    }

    public void Stop()
    {
        activeMusic?.Stop();
    }

    public void Pause()
    {
        activeMusic?.Pause();
    }

    public void UnPause()
    {
        activeMusic?.UnPause();
    }
}
