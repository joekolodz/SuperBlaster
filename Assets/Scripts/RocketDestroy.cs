using UnityEngine;

public class RocketDestroy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains(gameObject.name))
        {
            return;
        }

        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform));

        var trail = gameObject.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.Clear();
        }

        ObjectPooler.Instance.ReturnRocket(gameObject);
    }
}
