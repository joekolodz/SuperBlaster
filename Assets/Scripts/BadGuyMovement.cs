using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuyMovement : MonoBehaviour
{
    public Transform badGuy;
    public int moveSpeed = 5;
    public bool isDestroyed = false;

    private Transform radar;
    private Transform nextWaypoint;

    void Awake()
    {
        radar = GameObject.Find("Radar")?.transform;

        var waypointList = GameObject.Find("Waypoints");
        if (waypointList == null) return; //happens on main menu

        var navigation = waypointList.GetComponent<Navigation>();
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
    }

    //should be a utility right?
    private Vector3 GetDirection(Vector3 source, Vector3 target)
    {
        var direction = target - source;
        return direction;
    }


    //testing out a different way. doesn't quite work yet
    private Quaternion GetRotation(Transform source, Transform target, float turnSpeed)
    {
        var targetAngle = Vector3.Angle(source.position, target.position) - 90;
        //return Quaternion.Slerp(from.rotation, Quaternion.Euler(0, 0, targetAngle), turnSpeed * Time.deltaTime);

        //var targetRotation = Quaternion.LookRotation(GetDirection(source.position, target.position));
        var strength = Mathf.Min(turnSpeed * Time.deltaTime, 1);
        return Quaternion.Slerp(source.rotation, Quaternion.Euler(0, 0, targetAngle), strength);
    }

    void Update()
    {
        if (badGuy == null || nextWaypoint == null || radar == null) return;

        //rotate to face the radar target
        badGuy.rotation = GeometricFunctions.RotateToFace(badGuy.position, radar.position);

        //move towards the waypoint
        var direction = GetDirection(badGuy.position, nextWaypoint.position).normalized;

        badGuy.Translate(moveSpeed * direction * Time.deltaTime, Space.World);

        var distance = Vector3.Distance(badGuy.position, nextWaypoint.position);
        if (distance < 0.2f)
        {
            nextWaypoint = nextWaypoint.GetComponent<NextWaypoint>().GetNextWaypoint();
        }
    }

    public void RotateToFaceRadar(float withAccuracy)
    {
        if (badGuy == null || radar == null) return;

        var adjustAngle = new Vector3(0f, 0f, 0f);

        //account for accuracy and nudge the aim off a litte if necessary
        var isAccurate = Random.Range(0.0f, 1.0f) <= withAccuracy;

        if (isAccurate)
        {
            //this accounts for the rifle offset and points closer to the center of the radar
            adjustAngle.z = -7f;
        }
        else
        {
            //add cone of inaccuracy and account for accidental accuracy range around -7
            adjustAngle.z = Random.Range(-20.0f, +20.0f);
            if (adjustAngle.z < -0.0f && adjustAngle.z > -14.0f)
            {
                adjustAngle.z = -25f;
            }
        }

        //rotate to face the radar target
        badGuy.rotation = GeometricFunctions.RotateToFace(badGuy.position, radar.position);

        //account for final accuracy
        badGuy.Rotate(adjustAngle);
    }
}
