using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDestroy : MonoBehaviour
{
    public GameObject TheBase;
    public GameObject mainMenuPanel;

    void OnDestroy()
    {
        //GameObject.Find("MenuControl").GetComponent<MenuControl>().Pause();

        var fireControl = GameObject.Find("FireControl");
        if (fireControl)
        {
            fireControl.GetComponent<FireControl>().StopAllBadGuyMovement();
        }

        //var f = GameObject.Find("FireControl");
        //print(f);

        //call UI script to replay
        if (mainMenuPanel)
        {
            mainMenuPanel.SetActive(true);
        }
        TheBase.SetActive(false);
    }
}
