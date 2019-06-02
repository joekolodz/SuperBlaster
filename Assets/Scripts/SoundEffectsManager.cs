using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    private const int MaxPlasmaHitSounds = 1;
    public static new GameObject gameObject;
    private static AudioSource SmallExplosion;
    private static AudioSource LargeExplosion;
    private static AudioSource RocketLaunched; //RocketFire.cs
    private static List<AudioSource> RocketLaunchedList;
    private static List<AudioSource> PlasmaBlast; //PlasmaBlast.cs
    private static AudioSource PlasmaBlastPrefab;

    private static List<AudioSource> PlasmaHit; //PlasmaBlast.cs
    private static AudioSource PlasmaHitPrefab;
    private static GameObject PlasmaHitPool;

    private static AudioSource ObjectHit; //ObjectHit.cs
    private static AudioSource ObjectDestroy; //ObjectDestroy.cs

    public static void Initialize()
    {
        RocketLaunchedList = new List<AudioSource>();
        PlasmaBlast = new List<AudioSource>();
        PlasmaHit = new List<AudioSource>();
        CreateLocalGameObject();
        LoadPrefabs();
        AddHandlers();
    }

    private static void AddHandlers()
    {
        EventAggregator.PlasmaBlastHit += EventAggregator_PlasmaBlastHit;
    }

    private static void CreateLocalGameObject()
    {
        gameObject = new GameObject("SoundEffectsManager");
        gameObject.transform.position = new Vector3(0, 0, 0);

        PlasmaHitPool = new GameObject("Plasma Hit Pool");
        PlasmaHitPool.transform.SetParent(gameObject.transform, true);
    }

    private static void LoadPrefabs()
    {
        PlasmaHitPrefab = LoadSoundEffect("audio/SoundEffect - PlasmaHit");
        PlasmaBlastPrefab = LoadSoundEffect("audio/SoundEffect - PlasmaBlast");
    }

    private static AudioSource LoadSoundEffect(string resourceName)
    {
        var resourceRequest = Resources.LoadAsync<AudioSource>(resourceName);
        var prefabAsset = resourceRequest.asset as AudioSource;
        return prefabAsset;
    }

    private static void EventAggregator_PlasmaBlastHit(object sender, System.EventArgs e)
    {
        PlayPlasmaHit();
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
        }

        available.Play();

    }

    public static void PlayPlasmaHit()
    {
        AudioSource available = null;

        foreach (var source in PlasmaHit)
        {
            if (!source.isPlaying)
            {
                available = source;

                break;
            }
        }

        if (available == null || available.isPlaying)
        {
            if (PlasmaHit.Count > MaxPlasmaHitSounds) return;

            available = Instantiate(PlasmaHitPrefab);
            available.transform.SetParent(PlasmaHitPool.transform, true);
            PlasmaHit.Add(available);
        }

        available.Play();
    }
}