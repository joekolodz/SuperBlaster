using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Transform[] waypoints;

    // Use this for initialization
    void Awake()
    {
        var list = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[list.Length];
        for (var i = 0; i < list.Length; i++)
        {
            waypoints[i] = list[i].transform;
        }
    }
}
