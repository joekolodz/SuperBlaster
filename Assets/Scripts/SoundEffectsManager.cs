using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static readonly SoundEffectsManager Instance = (new GameObject("SoundEffectsManagerContainer")).AddComponent<SoundEffectsManager>();

    private bool _isInitialized = false;
    private const int MaxPlasmaHitSounds = 1;
    //private AudioSource SmallExplosion;
    //private AudioSource LargeExplosion;
    //private AudioSource RocketLaunched; //RocketFire.cs
    private List<AudioSource> RocketLaunchedList;
    private List<AudioSource> PlasmaBlast; //PlasmaBlast.cs
    private AudioSource PlasmaBlastPrefab;

    private List<AudioSource> PlasmaHit; //PlasmaBlast.cs
    private AudioSource PlasmaHitPrefab;
    private GameObject PlasmaHitPool;

    //private AudioSource ObjectHit; //ObjectHit.cs
    //private AudioSource ObjectDestroy; //ObjectDestroy.cs

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

    public void Initialize()
    {
        if (_isInitialized) return;
        RocketLaunchedList = new List<AudioSource>();
        PlasmaBlast = new List<AudioSource>();
        PlasmaHit = new List<AudioSource>();
        CreateLocalGameObject();
        LoadPrefabs();
        AddHandlers();
        _isInitialized = true;
    }

    private void AddHandlers()
    {
        EventAggregator.PlasmaBlastHit += EventAggregator_PlasmaBlastHit;
    }

    public void OnDestroy()
    {
        //SmallExplosion = null;
        //LargeExplosion = null;
        //RocketLaunched = null;
        RocketLaunchedList = null;
        PlasmaBlast = null;
        PlasmaBlastPrefab = null;
        PlasmaHit = null;
        PlasmaHitPrefab = null;
        PlasmaHitPool = null;
        //ObjectHit = null;
        //ObjectDestroy = null;
        EventAggregator.PlasmaBlastHit -= EventAggregator_PlasmaBlastHit;
        _isInitialized = false;
    }

    private void CreateLocalGameObject()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);

        PlasmaHitPool = new GameObject("Plasma Hit Pool");
        PlasmaHitPool.transform.SetParent(gameObject.transform, true);
    }

    private void LoadPrefabs()
    {
        PlasmaHitPrefab = LoadSoundEffect("audio/SoundEffect - PlasmaHit");
        PlasmaBlastPrefab = LoadSoundEffect("audio/SoundEffect - PlasmaBlast");
    }

    private AudioSource LoadSoundEffect(string resourceName)
    {
        var resourceRequest = Resources.LoadAsync<AudioSource>(resourceName);
        var prefabAsset = resourceRequest.asset as AudioSource;
        return prefabAsset;
    }

    private void EventAggregator_PlasmaBlastHit(object sender, System.EventArgs e)
    {
        PlayPlasmaHit();
    }

    
    //unused - remove?
    private void PlayRocketLaunched()
    {
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
            RocketLaunchedList.Add(available);
        }

        available.volume = 0.1f;
        available.Play();
    }

    private void PlayPlasmaHit()
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

        available.volume = 0.1f;
        available.Play();
    }
}