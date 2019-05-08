using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static readonly SoundEffectsManager Instance = (new GameObject("SoundEffectsManagerSingletonContainer")).AddComponent<SoundEffectsManager>();

    private AudioSource SmallExplosion;
    private AudioSource LargeExplosion;
    private AudioSource RocketLaunched; //RocketFire.cs
    private List<AudioSource> RocketLaunchedList = new List<AudioSource>();
    private List<AudioSource> PlasmaBlast = new List<AudioSource>(); //PlasmaBlast.cs
    private AudioSource PlasmaHit; //PlasmaBlast.cs
    private AudioSource ObjectHit; //ObjectHit.cs
    private AudioSource ObjectDestroy; //ObjectDestroy.cs

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SoundEffectsManager()
    {
    }

    private SoundEffectsManager()
    {
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private AudioSource LoadSoundEffect(string audioCliepResourceName)
    {
        var obj = Instance.gameObject;
        var audio = obj.AddComponent<AudioSource>();
        var resourceRequest = Resources.LoadAsync<AudioClip>(audioCliepResourceName);
        audio.clip = resourceRequest.asset as AudioClip;
        return audio;
    }

    private void PlayAudio(ref AudioSource source, string audioCliepResourceName)
    {
        if (source == null)
        {
            source = LoadSoundEffect(audioCliepResourceName);
        }

        if (source.isPlaying) return;

        source.Play();
    }

    public void PlayObjectDestroy()
    {
        //PlayAudio(ref ObjectDestroy, "audio/we beat one up");
    }

    public void PlayObjectHit()
    {
        //PlayAudio(ref ObjectHit, "audio/yeah");
    }

    public void PlayRocketLaunched()
    {
        //PlayAudio(ref RocketLaunched, "audio/rocket launched");

        AudioSource available = null;

        foreach (var source in RocketLaunchedList)
        {
            if (!source.isPlaying)
            {
                available = source;
                break;
            }
        }

        if (available == null || available.isPlaying)
        {
            available = LoadSoundEffect("audio/rocket launched");
            available.priority = 98;
            available.volume = 0.213f;
            RocketLaunchedList.Add(available);
            Debug.Log($"pitch: {available.pitch}");
        }

        available.Play();

    }

    public void PlayPlasmaHit()
    {
        PlayAudio(ref PlasmaHit, "audio/plasma hit");
    }

    public void PlayPlasmaBlast()
    {
        PlayAudio(ref PlasmaHit, "audio/plasma blast");
    }

    // play multiple similar sounds at the same time
    //
    //public void PlayPlasmaBlast()
    //{
    //    AudioSource available = null;

    //    foreach (var source in PlasmaBlast)
    //    {
    //        if (!source.isPlaying)
    //        {
    //            available = source;
    //            break;
    //        }
    //    }

    //    if (available == null || available.isPlaying)
    //    {
    //        Debug.Log($"no clips available creating a new one");
    //        available = LoadSoundEffect("audio/plasma blast");
    //        PlasmaBlast.Add(available);
    //        Debug.Log($"clip count: {PlasmaBlast.Count}");
    //    }

    //    available.Play();
    //}

}