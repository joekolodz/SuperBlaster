using UnityEngine;

public class GeometricFunctions
{
    public static Quaternion RotateToFace(Vector3 from, Vector3 to)
    {
        var direction = to - from;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static Quaternion RotateToFace(Vector3 from, Vector3 to, float angleAdjustment)
    {
        var direction = to - from;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle + angleAdjustment));
    }
}
