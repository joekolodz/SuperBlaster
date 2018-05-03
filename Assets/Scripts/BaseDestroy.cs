using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDestroy : MonoBehaviour
{
    public GameObject TheBase;

    void OnDestroy()
    {
        TheBase.SetActive(false);
        //call UI script to replay
        var panel = GameObject.Find("Panel (Dead)");
        panel.SetActive(true);

    }
}
