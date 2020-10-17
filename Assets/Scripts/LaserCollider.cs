using UnityEngine;

public class LaserCollider : MonoBehaviour
{
    public int Damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains(gameObject.name)) return;

        Debug.Log($"LaserCollider:{gameObject.name} hit by {collision.gameObject.name}");

        if (collision.gameObject.name.ToLower().Contains("wall"))
        {
            EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform));
            ObjectPooler.Instance.ReturnLaser(gameObject);
        }

        var obj = collision.gameObject.GetComponent<ObjectHit>();
        if (obj == null) return;
        obj.TakeDamage(Damage);
    }
}
