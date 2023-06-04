using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private int id;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collidingGameObject.name.Contains(gameObject.name) ||
        if (collision.gameObject.name.Contains("Rocket"))
        {
            Debug.Log($"Breakable Wall Section {id}");
            EventAggregator.PublishShowDebris(new ShowDebrisEventArgs(gameObject.transform));
            Destroy(gameObject);
        }
    }
}
