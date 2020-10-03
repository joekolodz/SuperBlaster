using UnityEngine;

public class LaserCollider : MonoBehaviour
{
    public int Damage = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject.GetComponent<ObjectHit>();
        if (obj != null)
        {
            Debug.Log("Laser Fucked!");
            obj.TakeDamage(Damage);
            return;
        }
    }
}
