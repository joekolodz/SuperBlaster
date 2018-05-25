using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyMovement : MonoBehaviour
{
    public Transform badGuy;
    public int moveSpeed = 5;

    private Transform nextWaypoint;
    private Vector3 direction;

    void Awake()
    {
        print("badguy movement - looking for waypoints");

        var navigation = GameObject.Find("Waypoints").GetComponent<Navigation>();
        var closestWaypointDistance = -1f;
        var closestWaypoint = navigation.waypoints[0];

        var waypoints = navigation.waypoints;

        foreach (var wp in waypoints)
        {
            var distance = Vector3.Distance(badGuy.position, wp.position);
            if (distance < closestWaypointDistance || closestWaypointDistance == -1)
            {
                closestWaypointDistance = distance;
                closestWaypoint = wp;
            }
        }
            
        nextWaypoint = closestWaypoint;
        direction = nextWaypoint.position - badGuy.position;
        //Debug.Log("Closest waypoint to " + badGuy.parent.name + " is " + nextWaypoint.name + ". Direction:" + direction);
    }

    void Update()
    {
        if (badGuy == null) return;

        //rotate to keep the next waypoint in view
        badGuy.rotation = GeometricFunctions.RotateToFace(badGuy.position, nextWaypoint.position, -90);
        direction = (nextWaypoint.position - badGuy.position).normalized;

        //move towards the waypoint
        badGuy.Translate(moveSpeed * direction * Time.deltaTime, Space.World);
        var distance = Vector3.Distance(badGuy.position, nextWaypoint.position);
        if (distance < .1f)
        {
            var newWaypoint = nextWaypoint.GetComponent<NextWaypoint>().GetNextWaypoint();
            nextWaypoint = newWaypoint;
            direction = nextWaypoint.position - badGuy.position;
        }
    }
}
