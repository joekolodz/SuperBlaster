using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyMovement : MonoBehaviour
{
    public Transform badGuy;
    public Transform[] waypoints;
    public int moveSpeed = 5;

    private Transform nextWaypoint;
    private Vector3 direction;

    void Awake()
    {
        var closestWaypointDistance = -1f;
        var closestWaypoint = waypoints[0];

        foreach(var wp in waypoints)
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
        Debug.Log("Closest waypoint to " + badGuy.parent.name + " is " + nextWaypoint.name + ". Direction:" + direction);
    }

    void Update()
    {
        if (badGuy == null) return;
        //if (!badGuy.parent.name.Contains("1")) return;

        rotate();

        badGuy.Translate(moveSpeed * direction * Time.deltaTime, Space.World);
        var distance = Vector3.Distance(badGuy.position, nextWaypoint.position);
        if (distance < .1f)
        {
            var newWaypoint = nextWaypoint.GetComponent<NextWaypoint>().GetNextWaypoint();
            nextWaypoint = newWaypoint;
            direction = nextWaypoint.position - badGuy.position;
        }
    }

    private void rotate()
    {
        direction = nextWaypoint.position - badGuy.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        badGuy.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        direction = direction.normalized;
    }
}
