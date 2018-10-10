using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    public  bool isPaused = false;

    private AudioSource _music = null;

    private void Start()
    {
        _music = GameObject.Find("Audio Source").GetComponent<AudioSource>();

    }
    void Update()
    {
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
