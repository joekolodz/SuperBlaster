using System.Collections.Generic;
using UnityEngine;

public class TitleSwap : MonoBehaviour
{
    public List<GameObject> titles = null;

    public enum TitleNames
    {
        Main,
        SpeedBlaster,
        TripleBlaster,
        MultiBlaster,
        SuperBlaster
    }

    
    public void ResetMainTitle()
    {
        titles[0].SetActive(true);
    }

    public void SetTitle(TitleNames title)
    {
        ClearTitle();
        switch (title)
        {
            case TitleNames.Main:
                titles[0].SetActive(true);
                break;
            case TitleNames.SpeedBlaster:
                titles[1].SetActive(true);
                break;
            case TitleNames.TripleBlaster:
                titles[2].SetActive(true);
                break;
            case TitleNames.MultiBlaster:
                titles[3].SetActive(true);
                break;
            case TitleNames.SuperBlaster:
                titles[4].SetActive(true);
                break;
        }
    }

    private void ClearTitle()
    {
        foreach (var title in titles)
        {
            title.SetActive(false);
        }
    }
}
