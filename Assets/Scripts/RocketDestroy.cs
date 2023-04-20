using UnityEngine;

public class RocketDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectHit(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectHit(collision.gameObject);
    }

    private void Update()
    {
        var x = gameObject.transform.position.x;
        var y = gameObject.transform.position.y;
        
        if (x > 50 || x < -48 || y > 25 || y < -23)
        {
            DestroyRocket();
        }
    }

    private void DetectHit(GameObject collidingGameObject)
    {
        if (collidingGameObject.name.Contains(gameObject.name) || collidingGameObject.name == "Rocket Spawn Point" || collidingGameObject.name == "Plasma Spawn Point")
        {
            return;
        }

        if (PowerUpManager.Instance.PowerUpType == PowerUpManager.PowerUpNames.SpeedBlaster && collidingGameObject.name == "Bad Guy")
        {
            return;
        }

        Debug.Log($"{gameObject.name} detected a hit from: {collidingGameObject.name}");
        DestroyRocket();
    }

    private void DestroyRocket()
    {
        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform));

        var trail = gameObject.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.Clear();
        }

        ObjectPooler.Instance.ReturnRocket(gameObject);
    }
}
