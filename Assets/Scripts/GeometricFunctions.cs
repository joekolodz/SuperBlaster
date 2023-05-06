using UnityEngine;

public class GeometricFunctions
{
    public static Quaternion RotateToFace(Vector3 from, Vector3 to)
    {
        var direction = to - from;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Quaternion RotateToFace(Vector3 from, Vector3 to, float angleAdjustment)
    {
        var direction = to - from;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle + angleAdjustment));
    }
    public static Vector3 GetPointWithOffset(Transform transform, Vector3 offset)
    {
        var result = Quaternion.Euler(transform.eulerAngles) * offset;
        return transform.position + result;
    }

    public static Vector3 GetAngleOfIncident(Transform transform, Vector3 offset)
    {
        return Quaternion.Euler(transform.eulerAngles) * offset;
    }

    public static Vector3 Reflect(Vector3 incident, Vector3 normal)
    {
        var incidentNormal = incident.normalized;
        var nPrime = Vector3.Dot(incidentNormal, normal) * normal;
        var r = 2 * nPrime;
        var reflection = incidentNormal - r;
        return reflection;
    }
}
