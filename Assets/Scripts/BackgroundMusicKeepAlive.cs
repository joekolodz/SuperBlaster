using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicKeepAlive : MonoBehaviour {

    private void Awake()
    {
        Debug.LogWarning("music Awake()");
        GameObject[] musicList = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if(musicList.Length>1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
