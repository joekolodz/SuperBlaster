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

        if (x > 50 || x < -50 || y > 25 || y < -25)
        {
            DestroyRocket();
        }
    }

    private void DetectHit(GameObject collidingGameObject)
    {
        //collidingGameObject.name.Contains(gameObject.name) ||
        if (
            collidingGameObject.name.Contains("Reflective Wall") ||
            collidingGameObject.name == "Rocket Spawn Point" ||
            collidingGameObject.name == "Plasma Spawn Point")
        {
            return;
        }

        if (PowerUpManager.Instance.PowerUpType == PowerUpManager.PowerUpNames.SpeedBlaster && collidingGameObject.name == "Bad Guy")
        {
            return;
        }

        DestroyRocket();
    }

    private void DestroyRocket()
    {
        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform));
        ObjectPooler.Instance.ReturnRocket(gameObject);
    }
}
