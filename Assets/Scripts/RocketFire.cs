using UnityEngine;

public class RocketFire : MonoBehaviour
{
    public AudioSource audioSource;

    public float _currentForce;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _currentForce = 0;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void FireRocket(float force)
    {
        _currentForce += force;
        _rigidBody.AddForce(gameObject.transform.right * force);
    }

    public void StopRocket()
    {
        _rigidBody.AddForce(-gameObject.transform.right * _currentForce);
        _currentForce = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FirePoint"))
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }
    }
}