using UnityEngine;

public class RocketFire : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FirePoint"))
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }
    }
}