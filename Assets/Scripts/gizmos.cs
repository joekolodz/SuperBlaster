using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Assets.Scripts
{
    public class gizmos : MonoBehaviour
    {

        #region Vector Reflection
        [SerializeField, Description("test")]
        private Vector3 incident = new Vector3(10.0f, 5.0f);
        [SerializeField]
        private Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);

        #endregion

        public void OnDrawGizmos()
        {
            //FindRotation();

            #region Vector Reflection

            //FirstTry();
            //SecondTry();
            //ThirdTry();
            //FourthTry();

            #endregion
        }

        
        private void FindRotation()
        {
            Gizmos.color = Color.yellow;

            origin = transform.position;

            Gizmos.DrawSphere(origin, 0.3f);

            var s = GameObject.Find("Shield Template (4)");

            var rotation = s.transform.rotation;

            var newPoint = new Vector3(2.3f, 0.0f, 0.0f);
            var rotatedPoint = Quaternion.AngleAxis(45.0f, Vector3.forward) * newPoint;
            var rotatedPointWorld = transform.TransformPoint(rotatedPoint);

            var rayOrigin = new Vector3(4.3f, 0.0f, 0.0f);
            //var rayRotated = Quaternion.Euler(s.transform.eulerAngles) * rayOrigin;
            var rayRotated = Quaternion.AngleAxis(s.transform.rotation.z, Vector3.up) * rayOrigin;
            var rayRotatedWorld = transform.TransformPoint(rayRotated);


            Debug.Log($"origin world: {transform.position}");
            Debug.Log($"newPoint: {newPoint}");
            Debug.Log($"rotatedPoint: {rotatedPoint}");
            Debug.Log($"rotatedPointWorld: {rotatedPointWorld}");


            Debug.Log($"rayOrigin: {rayOrigin}");
            Debug.Log($"shield angles: {s.transform.eulerAngles}");
            Debug.Log($"shield Q: {s.transform.rotation}");
            Debug.Log($"rayRotated: {rayRotated}");
            Debug.Log($"rayRotatedWorld: {rayRotatedWorld}");

            //origin test
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(newPoint, 0.25f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(rotatedPoint, 0.15f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(rotatedPointWorld, 0.15f);


            //ray test
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(rayOrigin, 0.25f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(rayRotated, 0.15f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(rayRotatedWorld, 0.15f);



        }

        private void FourthTry()
        {
            DrawGizmos(origin, incident);
        }

        #region Vector Reflection

        /// <summary>
        /// Use first collider result from raycast
        /// </summary>
        /// <param name="objectOrigin"></param>
        /// <param name="objectIncident"></param>
        public static void DrawRays(Vector3 objectOrigin, Vector3 objectIncident)
        {
            var hit = Physics2D.Raycast(objectOrigin, objectIncident);

            Debug.DrawLine(objectOrigin, hit.point, Color.red);

            Debug.DrawRay(objectOrigin, objectIncident, Color.white);

            var normal = hit.normal;

            var reflection = Reflect2(objectIncident, normal);
            Debug.DrawRay(hit.point, reflection * 10, Color.green);

            //draw hit normal
            Debug.DrawLine(hit.point, hit.point + new Vector2(normal.x, normal.y) * 5, Color.cyan);
        }

        /// <summary>
        /// Use first collider result from raycast that isn't the gameObject passed in
        /// </summary>
        /// <param name="objectOrigin"></param>
        /// <param name="objectIncident"></param>
        /// <param name="excludeThisGameObject"></param>
        public static void DrawRays(Vector3 objectOrigin, Vector3 objectIncident, GameObject excludeThisGameObject)
        {
            var hits = Physics2D.RaycastAll(objectOrigin, objectIncident);
            RaycastHit2D hit = default;

            foreach (var h in hits)
            {
                if (h.transform.gameObject == excludeThisGameObject) continue;

                hit = h;
                break;
            }

            if (hit == default) return;

            Debug.DrawLine(objectOrigin, hit.point, Color.red);
            Debug.DrawRay(objectOrigin, objectIncident, Color.white);

            var normal = hit.normal;

            var reflection = Reflect2(objectIncident, normal);
            Debug.DrawRay(hit.point, reflection * 5, Color.green);

            //draw hit normal
            Debug.DrawLine(hit.point, hit.point + new Vector2(normal.x, normal.y) * 5, Color.cyan);
        }


        public static void DrawGizmos(Vector3 objectOrigin, Vector3 objectIncident)
        {
            var hit = Physics2D.Raycast(objectOrigin, objectIncident);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 0.3f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(objectOrigin, hit.point);

            Gizmos.color = Color.white;
            Gizmos.DrawRay(objectOrigin, objectIncident);


            var normal = hit.normal;

            var reflection = Reflect2(objectIncident, normal);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(hit.point, reflection * 10);

            //draw hit normal
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(hit.point, hit.point + new Vector2(normal.x, normal.y) * 5);
        }

        private void ThirdTry()
        {
            var normal = new Vector3(0.0f, 1.0f);
            var reflection = Reflect2(incident, normal);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, incident);

            Gizmos.color = Color.white;
            Gizmos.DrawRay(origin, reflection * 10);

        }

        private void SecondTry()
        {
            //var incident = new Vector3(10.0f, 5.0f);
            var reflected = new Vector3(-10.0f, 5.0f);
            var normal = new Vector3(0.0f, -1.0f);

            var incidentNormal = incident.normalized;
            var reflectedNormal = reflected.normalized;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, incident);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(origin, reflected);

            Gizmos.color = Color.blue;
            //Gizmos.DrawRay(origin, new Vector3(0.0f, 10.0f, 0.0f));
            Gizmos.DrawRay(origin, normal);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(origin, 1.0f);

            var uPrime = reflectedNormal - (Vector3.Dot(reflectedNormal, normal) * normal);
            var uuPrime = incidentNormal - (Vector3.Dot(incidentNormal, normal) * normal);
            var ooPrime = -uuPrime;

            var nPrime = Vector3.Dot(incidentNormal, normal) * normal;

            //now calculate the reflection
            var reflection = 2 * nPrime - incidentNormal;

            Gizmos.color = Color.white;
            Gizmos.DrawRay(origin, reflection * 10);

        }

        private static Vector3 Reflect2(Vector3 incident, Vector3 normal)
        {
            var incidentNormal = incident.normalized;
            var nPrime = Vector3.Dot(incidentNormal, normal) * normal;
            //var reflection = 2 * nPrime - incidentNormal;
            var r = 2 * nPrime;
            var reflection = incidentNormal - r;
            return reflection;
        }

        private void FirstTry()
        {
            //origin position of rocket
            var rocket = new Vector3(20.0f, 10.0f);

            //direction of rocket
            var rocketDirection = new Vector3(7.0f, -30.0f);
            //direction of rocket normalized
            var direction = new Vector3(7.0f, -30.0f).normalized;

            //ray in direction of rocket
            Gizmos.color = Color.yellow;
            var ray = new Ray(rocket, direction);
            Gizmos.DrawRay(ray);


            //raycast of rocket
            var hit = Physics2D.Raycast(rocket, direction);


            //draw hit location
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hit.point, 1.0f);

            //draw ray from rocket to hit location
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(rocket, hit.point);

            //var w = GameObject.Find("Wall Piece (6)");


            //draw hit normal
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal);


            var proj = Vector3.Dot(hit.normal, direction);
            Vector3 projVector = (2 * Vector3.Dot(hit.normal, direction)) * hit.normal;

            //draw projected vector
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hit.point, projVector);

            var reflected = direction - projVector;

            //draw reflected vector
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(hit.point, reflected);

            //some dot
            Gizmos.DrawSphere(transform.position, 1);


            var reflected2 = Reflect(rocket.normalized, hit.normal);


            Gizmos.color = Color.green;
            Gizmos.DrawLine(hit.point, reflected2);
        }

        private Vector2 Reflect(Vector3 a, Vector3 b)
        {
            //return (2 * Vector3.Dot(a, b) / Vector3.Dot(b, b)) * b - a;
            return 2 * Vector3.Dot(b, a) * b - a;
        }
        #endregion

    }
}
