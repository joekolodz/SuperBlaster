using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class ReflectiveWall : MonoBehaviour
    {
        public void OnDrawGizmos()
        {
            var collider = GetComponent<BoxCollider2D>();
            var rocketIn = GameObject.Find("***IncomingRocket");
            var rocketOut = GameObject.Find("***OutgoingRocket");

            if (rocketIn == null || rocketOut == null) return;
            var rocketCollider = rocketIn.GetComponent<PolygonCollider2D>();


            if (!collider)
            {
                return; // nothing to do without a collider
            }

            //ROCKET
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(rocketIn.transform.position, 0.1f);

            //CLOSEST POINT
            Vector3 closestPoint = collider.ClosestPoint(rocketIn.transform.position);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(closestPoint, 0.1f);

            
            //ROCKET TIP
            var tip = GeometricFunctions.GetPointWithOffset(rocketIn.transform, new Vector3(-1.0f, 0.0f, 0.0f));
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(tip, 0.1f);

            //ROCKET TO COLLIDER CLOSEST POINT
            var closestPointToTip = GetComponent<BoxCollider2D>().ClosestPoint(tip);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(closestPointToTip, 0.1f);

            //INCIDENT ANGLE OF ROCKET
            var incident = GeometricFunctions.GetAngleOfIncident(rocketIn.transform, new Vector3(1.2f, 0.0f, 0.0f));


            //RAYCAST
            var hits = Physics2D.RaycastAll(tip, incident);
            RaycastHit2D hit = default;

            foreach (var h in hits)
            {
                Debug.Log($"Distance: {h.distance}");

                if (h.transform.gameObject == rocketIn.gameObject) continue;
                //if (h.distance > 2.9) continue;

                hit = h;
                break;
            }


            //NORMAL
            var normal = hit.normal;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(hit.point, hit.point + new Vector2(normal.x, normal.y) * 2);


            //REFLECTION POINT
            var reflection = GeometricFunctions.Reflect(incident, normal);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(reflection, 0.1f);

            //REFLECTION ANGLE
            Gizmos.color = Color.green;
            Gizmos.DrawRay(hit.point, reflection * 1.5f);

            var reflectionRocket = reflection + new Vector3(hit.point.x, hit.point.y);
            var targetRotation = GeometricFunctions.RotateToFace(hit.point, reflectionRocket);
            rocketOut.transform.position = rocketIn.transform.position;
            rocketOut.transform.localRotation = targetRotation;

        }

        private void OnTriggerEnter2D(Collider2D collidingObject)
        {
            var rb = collidingObject.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            var tip = GeometricFunctions.GetPointWithOffset(collidingObject.transform, new Vector3(-1.0f, 0.0f, 0.0f));
            var closestPointToTip = GetComponent<BoxCollider2D>().ClosestPoint(tip);
            var incident = GeometricFunctions.GetAngleOfIncident(collidingObject.transform, new Vector3(1.2f, 0.0f, 0.0f));

            var hits = Physics2D.RaycastAll(tip, incident);
            RaycastHit2D hit = default;

            foreach (var h in hits)
            {
                if (h.transform.gameObject == collidingObject.gameObject) continue;
                //if (h.distance > 2.9) continue;
                //Debug.Log($"Distance: {h.distance}");
                hit = h;
                break;
            }

            var reflection = GeometricFunctions.Reflect(incident, hit.normal);

            //DrawBox("*** tip", tip, Color.cyan);
            //DrawBox("*** closestPointToTip", closestPointToTip, Color.yellow);
            //DrawBox("*** hitPoint", hit.point, Color.green);
            //Debug.DrawRay(collidingObject.transform.position, incident, Color.white);
            //Debug.DrawRay(hit.point, reflection * 5, Color.green);
            //Debug.DrawLine(hit.point, hit.point + new Vector2(hit.normal.x, hit.normal.y) * 2, Color.cyan);

            collidingObject.attachedRigidbody.velocity = Vector2.zero;
            collidingObject.transform.position = hit.point;
            var reflectionRocket = reflection + new Vector3(hit.point.x, hit.point.y);//get a vector pointing from the hit point to the reflection point
            var targetRotation = GeometricFunctions.RotateToFace(hit.point, reflectionRocket);
            collidingObject.transform.localRotation = targetRotation;
            collidingObject.attachedRigidbody.AddForce(collidingObject.transform.right * 3000);
        }        


        private void DrawBox(string name, Vector3 position, Color color)
        {
            var origin = new GameObject(name)
            {
                hideFlags = HideFlags.None,
                isStatic = false,
                transform = { position = position, localScale = new Vector3(12.0f, 12.0f, 12.0f) }
            };

            origin.AddComponent<SpriteRenderer>();
            var r = origin.GetComponent<SpriteRenderer>();
            r.color = color;
            r.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0.0f, 0.0f, 1.0f, 1.0f), Vector2.zero);
            r.sortingOrder = 200;
        }
    }
}
