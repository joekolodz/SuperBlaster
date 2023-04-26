using UnityEngine;

public class PlasmaDestroy : MonoBehaviour
{
    [Range(0, 3)]
    public float explosionScale = 1.0f;
    public AudioSource audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (!audioSource.isPlaying && audioSource.enabled)
        //{
        //    //audioSource.Play();
        //    AudioSource.PlayClipAtPoint(audioSource.clip, new Vector3(0, 0, 0), audioSource.volume);
        //}
        EventAggregator.PublishPlasmaBlastHit();
        EventAggregator.PublishObjectDestroyed(new ObjectDestroyedEventArgs(gameObject.transform, explosionScale));

        var trail = gameObject.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.Clear();
        }

        ReturnPlasma();
    }

    private void ReturnPlasma()
    {
        ObjectPooler.Instance.ReturnPlasma(gameObject);      
    }
}
