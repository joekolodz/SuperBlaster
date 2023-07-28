using System;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : BaseSingleton<SoundEffectsManager>
{
    public Guid Id = Guid.NewGuid();
    private bool _isPoolPopulated = false;

    private GameObject PlasmaHitPool;
    private Dictionary<int, AudioSource> _plasmaHitPool;
    private AudioSource PlasmaHitPrefab;
    private int _playIndex = 0;
    private int _maxPlayIndex = 9;

    //private constructor is called before static constructor
    private SoundEffectsManager()
    {
    }

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SoundEffectsManager()
    {
    }

    public void Initialize()
    {
        if (_isPoolPopulated) return;

        PlasmaHitPrefab = LoadSoundEffect("audio/SoundEffect - PlasmaHit");
        PlasmaHitPool = new GameObject("Plasma Hit Pool");
        _plasmaHitPool = new Dictionary<int, AudioSource>();
        PlasmaHitPool.transform.SetParent(gameObject.transform, true);

        PopulatePool();

        AddHandlers();
        _isPoolPopulated = true;
    }

    private void PopulatePool()
    {
        for (var i = 0; i <= _maxPlayIndex; i++)
        {
            var p = Instantiate(PlasmaHitPrefab);
            p.transform.SetParent(PlasmaHitPool.transform, true);
            _plasmaHitPool.Add(i, p);
        }
    }

    private static AudioSource LoadSoundEffect(string resourceName)
    {
        var resourceRequest = Resources.LoadAsync<AudioSource>(resourceName);
        var prefabAsset = resourceRequest.asset as AudioSource;
        return prefabAsset;
    }

    private void AddHandlers()
    {
        EventAggregator.PlasmaBlastHit += EventAggregator_PlasmaBlastHit;
    }

    private void EventAggregator_PlasmaBlastHit(object sender, System.EventArgs e)
    {
        var a = _plasmaHitPool[_playIndex];
        if (a == null) return;

        a.Play();

        _playIndex++;
        if (_playIndex > _maxPlayIndex)
        {
            _playIndex = 0;
        }
    }
}