using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;

    public static MusicManager GetInstance()
    {
        return instance;
    }

    public System.Collections.Generic.List<AudioSource> PlayList;

    private static AudioSource activeMusic;
    private static System.Collections.Generic.List<AudioSource> basePlayList;
    private static bool isMainMenuMusicPlaying = false;

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            //Returning to the main menu will attempt to re-create a new music manager. do not allow that.
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            basePlayList = PlayList;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void SceneManager_activeSceneChanged(Scene current, Scene next)
    {
        if (!string.IsNullOrWhiteSpace(next.name))
        {
            if(next.buildIndex == 0)
            {
                Stop();
                activeMusic = basePlayList[0];
                Play();
                isMainMenuMusicPlaying = true;
            }
            else
            {
                if(isMainMenuMusicPlaying)
                {
                    Stop();
                    activeMusic = basePlayList[1];//could be random or based on level
                    Play();
                    isMainMenuMusicPlaying = false;
                }
            }
        }
    }

    public static void Play()
    {
        activeMusic?.Play();
    }

    public static void Play(int sceneIndex)
    {
        activeMusic?.Play();
    }

    public static void Stop()
    {
        activeMusic?.Stop();
    }

    public static void Pause()
    {
        activeMusic?.Pause();
    }

    public static void UnPause()
    {
        activeMusic?.UnPause();
    }
}
