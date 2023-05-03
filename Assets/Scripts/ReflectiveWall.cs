using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class ReflectiveWall : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collidingObject)
        {
            var rb = collidingObject.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            var rocketPosition = collidingObject.transform.position;
            var incident = FindRaycastOriginPoint2(collidingObject.transform);

            //gizmos.DrawRays(rocketPosition, incident, collidingObject.gameObject);

            var hits = Physics2D.RaycastAll(rocketPosition, incident);
            RaycastHit2D hit = default;

            foreach (var h in hits)
            {
                if (h.transform.gameObject == collidingObject.gameObject) continue;

                hit = h;
                break;
            }

            if (hit == default) return;
            var normal = hit.normal;
            var hitPoint = new Vector3(hit.point.x, hit.point.y, 0.0f);
            var reflection = Reflect(incident, normal);

            //Debug.DrawRay(rocketPosition, incident, Color.white);
            //Debug.DrawRay(hit.point, reflection * 5, Color.yellow);
            //Debug.DrawLine(hit.point, hit.point + new Vector2(normal.x, normal.y) * 5, Color.cyan);

            collidingObject.attachedRigidbody.velocity = Vector2.zero;
            //collidingObject.transform.position = hitPoint;
            rocketPosition = collidingObject.transform.position;//since we changed it



            var reflectionRocket = reflection + hitPoint;//get a vector pointing from the hit point to the reflection point

            var targetRotation = GeometricFunctions.RotateToFace(rocketPosition, reflectionRocket);

            collidingObject.transform.localRotation = targetRotation;

            collidingObject.attachedRigidbody.AddForce(collidingObject.transform.right * 3000);

            
            
            //trigger spark effects!!
            //trigger spark effects!!
            //trigger spark effects!!
        }

        private void ArcCosing(Collider2D collidingObject)
        {
            //get normal of the wall by rotating 90degrees counterclockwise from the right facing vector
            //swap x and y, then negate x
            var x = transform.right.y;
            var y = transform.right.x;
            x *= -1;
            var normal = new Vector3(x, y, 0.0f);
            var rocketPosition = collidingObject.transform.position;

            //take the angle between 2 vectors, the rocket vector and normal (then double it because that's only half way)
            //α = arccos[(xa xb + ya yb) / (√(xa² +ya²) × √(xb² +yb²))]
            //convert rocket position to the wall's local coords
            //take the normal of the wall (local)
            //do that crazy formula
            //apply rotation to the rocket
            //
            //PROBLEM: doesn't tell us which way to rotate the rocket

            var rocketLocal = transform.InverseTransformPoint(rocketPosition);

            var dot = Vector3.Dot(rocketLocal.normalized, normal);
            var angle = Mathf.Acos(dot);

            var angle2 = angle / Mathf.PI;
            var angle3 = 180 * angle2;
            

            Debug.Log($"Wall:{transform.position}, Rocket World:{rocketPosition}, Rocket Local:{rocketLocal}, Wall Normal:{normal}, Angle:{angle3}");

            collidingObject.transform.Rotate(0.0f, 0.0f, angle3);
        }

        private Vector3 FindRaycastOriginPoint2(Transform transform)
        {
            var rayOrigin = new Vector3(1.0f, 0.0f, 0.0f);
            var rayRotated = Quaternion.Euler(transform.eulerAngles) * rayOrigin;
            //var rayRotatedWorld = transform.position + rayRotated;

            //Debug.Log($"rocket world: {transform.position}");
            //Debug.Log($"rocket rotation: {transform.rotation}");
            //Debug.Log($"rocket angles: {transform.eulerAngles}");
            //Debug.Log($"rayOrigin: {rayOrigin}");
            //Debug.Log($"rayRotated: {rayRotated}");
            //Debug.Log($"rayRotatedWorld: {rayRotatedWorld}");

            //return rayRotatedWorld;
            return rayRotated;
        }

        //need to start the raycast past the collider
        private Vector3 FindRaycastOriginPoint(Transform transform)
        {
            var rayOrigin = new Vector3(1.2f, 0.0f, 0.0f);
            var rayRotated = Quaternion.Euler(transform.eulerAngles) * rayOrigin;
            var rayRotatedWorld = transform.position + rayRotated;

            //Debug.Log($"rocket world: {transform.position}");
            //Debug.Log($"rocket rotation: {transform.rotation}");
            //Debug.Log($"rocket angles: {transform.eulerAngles}");
            //Debug.Log($"rayOrigin: {rayOrigin}");
            //Debug.Log($"rayRotated: {rayRotated}");
            //Debug.Log($"rayRotatedWorld: {rayRotatedWorld}");

            return rayRotatedWorld;
        }

        private Vector3 Reflect(Vector3 incident, Vector3 normal)
        {
            var incidentNormal = incident.normalized;
            var nPrime = Vector3.Dot(incidentNormal, normal) * normal;
            var r = 2 * nPrime;
            var reflection = incidentNormal - r;
            return reflection;
        }

    }
}
