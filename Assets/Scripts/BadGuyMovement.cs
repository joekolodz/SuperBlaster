using UnityEngine;

public class BadGuyMovement : MonoBehaviour
{
    public Transform badGuy;
    public int moveSpeed = 5;
    public bool isDestroyed = false;
    public bool IsNextWaypointRandom = false;
    public int Id = 0;

    private Transform radar;
    private Transform nextWaypoint;
    private Navigation navigation;
    private Transform[] waypoints;
    private float AccuracyAdjustmentAngle = -11.0f;
    private bool isInitialized = false;


    public void Reset()
    {
        isInitialized = false;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        radar = GameObject.Find("Radar")?.transform;
        var p = gameObject.GetComponent<PlasmaSpawn>();
        if(p) p.isFiring = false;

        var waypointList = GameObject.Find("Waypoints");
        if (waypointList == null) return; //happens on main menu

        navigation = waypointList.GetComponent<Navigation>();
        isInitialized = true;
    }

    private void OnEnable()
    {
        if (!isInitialized) Initialize();
        FindNearestWaypoint();
    }

    private void FindNearestWaypoint()
    {
        if (navigation == null) return;

        waypoints = navigation.waypoints;

        var closestWaypointDistance = -1f;
        var closestWaypoint = navigation.waypoints[0];

        foreach (var wp in waypoints)
        {
            wp.gameObject.DrawCircle();

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
        gameObject.DrawCircle(new Color(1, 1, 0));

        if (badGuy == null || nextWaypoint == null || radar == null) return;

        badGuy.gameObject.DrawCircle(new Color(0, 1, 1));

        //rotate to face the radar target
        badGuy.rotation = GeometricFunctions.RotateToFace(badGuy.position, radar.position);
        badGuy.Rotate(new Vector3(0f, 0f, AccuracyAdjustmentAngle));

        //move towards the waypoint
        var direction = GetDirection(badGuy.position, nextWaypoint.position).normalized;

        badGuy.Translate(moveSpeed * direction * Time.deltaTime, Space.World);

        var distance = Vector3.Distance(badGuy.position, nextWaypoint.position);
        if (distance < 2f)
        {
            nextWaypoint.gameObject.DrawCircle(new Color(1, 0, 0));
            if (IsNextWaypointRandom)
            {
                nextWaypoint = navigation.GetRandomWaypoint(nextWaypoint);
            }
            else
            {
                nextWaypoint = nextWaypoint.GetComponent<NextWaypoint>().GetNextWaypoint();
            }
        }
        nextWaypoint.gameObject.DrawCircle(new Color(0, 1, 0));
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
            adjustAngle.z = AccuracyAdjustmentAngle;
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
