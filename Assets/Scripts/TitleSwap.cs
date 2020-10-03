using System.Collections.Generic;
using UnityEngine;

public class TitleSwap : MonoBehaviour
{
    public List<GameObject> titles;
    private Animator animator;

    public enum TitleNames
    {
        Main,
        SpeedBlaster,
        TripleBlaster,
        MultiBlaster,
        LaserBlaster,
        SuperBlaster
    }

    public void Awake()
    {
        EventAggregator.PowerUpExpired += EventAggregator_PowerUpExpired;
        EventAggregator.PowerUpTriggered += EventAggregator_PowerUpTriggered;
    }

    public void OnDestroy()
    {
        EventAggregator.PowerUpExpired -= EventAggregator_PowerUpExpired;
        EventAggregator.PowerUpTriggered -= EventAggregator_PowerUpTriggered;
    }

    private void EventAggregator_PowerUpTriggered(object sender, PowerUpTriggeredEventArgs e)
    {
        TitleNames titleName = TitleNames.Main;
        switch (e.PowerUpName)
        {
            case PowerUpManager.PowerUpNames.SpeedBlaster:
                titleName = TitleNames.SpeedBlaster;
                break;
            case PowerUpManager.PowerUpNames.TripleBlaster:
                titleName = TitleNames.TripleBlaster;
                break;
            case PowerUpManager.PowerUpNames.MultiBlaster:
                titleName = TitleNames.MultiBlaster;
                break;
            case PowerUpManager.PowerUpNames.LaserBlaster:
                titleName = TitleNames.LaserBlaster;
                break;
            case PowerUpManager.PowerUpNames.SuperBlaster:
                titleName = TitleNames.SuperBlaster;
                break;
        }
        SetTitle(titleName);
    }

    private void EventAggregator_PowerUpExpired(object sender, System.EventArgs e)
    {
        ResetMainTitle();
    }

    public void ResetMainTitle()
    {
        if (titles[0].activeSelf) return;

        ClearTitle();
        SetTitle(TitleNames.Main);
    }

    public void SetTitle(TitleNames title)
    {        
        switch (title)
        {
            case TitleNames.Main:
                titles[0].SetActive(true);
                animator = titles[0].GetComponent<Animator>();
                animator.SetTrigger("Spin");
                break;
            case TitleNames.SpeedBlaster:
                AnimatePowerUpTitle(titles[1]);
                break;
            case TitleNames.TripleBlaster:
                AnimatePowerUpTitle(titles[2]);
                break;
            case TitleNames.MultiBlaster:
                AnimatePowerUpTitle(titles[3]);
                break;
            case TitleNames.LaserBlaster:
                AnimatePowerUpTitle(titles[4]);
                break;
            case TitleNames.SuperBlaster:
                AnimatePowerUpTitle(titles[5]);
                break;
        }
    }

    private void AnimatePowerUpTitle(GameObject title)
    {
        ClearTitle();
        title.SetActive(true);
        animator = title.GetComponent<Animator>();
        animator.SetTrigger("Bloat");
    }

    private void ClearTitle()
    {
        foreach (var title in titles)
        {
            title.SetActive(false);
        }
    }
}
