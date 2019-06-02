using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Transform[] waypoints;
    private int _waypointCount;

    // Use this for initialization
    void Awake()
    {
        var list = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[list.Length];
        for (var i = 0; i < list.Length; i++)
        {
            //test
            waypoints[i] = list[i].transform;
        }
        _waypointCount = waypoints.Length;
    }

    public Transform GetRandomWaypoint(Transform currentWaypoint)
    {
        var newWaypoint = currentWaypoint;
        while(newWaypoint == currentWaypoint)
        {
            newWaypoint = waypoints[Random.Range(0, _waypointCount)];
        }

        return newWaypoint;
    }
}
