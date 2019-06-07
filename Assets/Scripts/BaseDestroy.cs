using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDestroy : MonoBehaviour
{
    public GameObject TheBase;
    public GameObject mainMenuPanel;

    void OnDestroy()
    {
        EventAggregator.PublishBaseDestroyed();
    }
}
