using Assets.Scripts;
using UnityEngine;
using UnityEngine.Rendering;

public class RocketFire : MonoBehaviour
{
    public AudioSource audioSource;

    public float _currentForce;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private void Awake()
    {
        _currentForce = 0;
        _rigidBody = GetComponent<Rigidbody2D>();

        _collider = GetComponent<Collider2D>();

        if (PowerUpManager.Instance.PowerUpType == PowerUpManager.PowerUpNames.SpeedBlaster)
        {
           _collider.isTrigger = true;
        }
    }

    void Update()
    {
        if (PowerUpManager.Instance.PowerUpType == PowerUpManager.PowerUpNames.SpeedBlaster && _currentForce > 0)
        {
            gameObject.transform.position += transform.right * 250 * Time.deltaTime;
        }
    }

    public void FireRocket(float force)
    {
        _currentForce += force;
        
        if (PowerUpManager.Instance.PowerUpType == PowerUpManager.PowerUpNames.SpeedBlaster) return;
        _rigidBody.AddForce(gameObject.transform.right * force);
    }

    public void StopRocket()
    {
        _currentForce = 0;
        _rigidBody.AddForce(-gameObject.transform.right * _currentForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FirePoint"))
        {
            if (audioSource.isPlaying)
            {
                return;
            }
            audioSource.Play();
        }
    }
}