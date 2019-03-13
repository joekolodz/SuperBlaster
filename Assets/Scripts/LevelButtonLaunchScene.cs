using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonLaunchScene : MonoBehaviour
{
    public int sceneIndex = 0;
    public GameObject panelLevels;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    public void StartNewGame()
    {        
        panelLevels.SetActive(false);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
