using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicKeepAlive : MonoBehaviour {

    private void Awake()
    {
        GameObject[] musicList = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if(musicList.Length>1)
        {
            //Debug.LogWarning("music Awake() - Destroying Length>1");
            //var audio = musicList[0].GetComponent<AudioSource>();
            //Debug.LogWarning($"music list [0] playing? : {audio.isPlaying}");
            //audio = musicList[1].GetComponent<AudioSource>();
            //Debug.LogWarning($"music list [1] playing? : {audio.isPlaying}");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public static void StopMusic()
    {
        GameObject[] musicList = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if(musicList.Length==1)
        {
            var audio = musicList[0].GetComponent<AudioSource>();
            audio.Stop();           
        }
    }
}
