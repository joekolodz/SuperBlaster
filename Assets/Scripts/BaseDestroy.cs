using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDestroy : MonoBehaviour
{
    public GameObject TheBase;
    public GameObject mainMenuPanel;

    void OnDestroy()
    {
        Debug.Log("Base Destroyed");
        StateManager.isWaitingForNextLevelToStart = true;
        Time.timeScale = 0.25f;
        PowerUp.Instance.ResetPowerUp();

        var fireControl = GameObject.Find("FireControl");
        if (fireControl)
        {
            fireControl.GetComponent<FireControl>().StopAllBadGuyMovement();
        }

        //call UI script to replay
        if (mainMenuPanel)
        {
            mainMenuPanel.SetActive(true);
        }
        TheBase.SetActive(false);
    }
}
