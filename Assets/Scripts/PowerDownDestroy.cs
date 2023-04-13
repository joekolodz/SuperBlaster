using UnityEngine;

public class PowerDownDestroy : MonoBehaviour
{
    public GameObject explosion;
    public GameObject hitTriggerObject;
    public float delayDestroy = 1.8f;
    public AudioSource soundOnDestroy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains(hitTriggerObject.name))
        {
            PowerDownManager.Instance.PowerDownHit();

            AudioSource.PlayClipAtPoint(soundOnDestroy.clip, new Vector3(0, 0, 0));

            Explode();
        }
    }

    private void Explode()
    {
        var obj = Instantiate(explosion, transform.position, Quaternion.identity);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        StartCoroutine(WaitForTime.Wait(delayDestroy, () =>
        {
            Destroy(obj);
            Destroy(gameObject);
        }));
    }
}