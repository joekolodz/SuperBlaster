using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Transform[] waypoints;
    private int _waypointCount;

    // Use this for initialization
    void Awake()
    {
        _waypointCount = gameObject.transform.childCount;
        waypoints = new Transform[_waypointCount];
        for (var i = 0; i < _waypointCount; i++)
        {
            waypoints[i] = gameObject.transform.GetChild(i);
        }
    }

    public Transform GetRandomWaypoint(Transform currentWaypoint)
    {
        var newWaypoint = currentWaypoint;
        while (newWaypoint == currentWaypoint)
        {
            newWaypoint = waypoints[Random.Range(0, _waypointCount)];
        }

        return newWaypoint;
    }
}
