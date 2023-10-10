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

        if (PowerUpManager.Instance.PowerUpType == PowerUpManager.PowerUpNames.SpeedBlaster) return;//SpeedRocket movement is handled in the Update
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
            if (audioSource.isPlaying || !audioSource.isActiveAndEnabled)
            {
                return;
            }

            Debug.Log("RocketFire - OnTriggerEnter2D - After Audio Check");

            switch (PowerUpManager.Instance.PowerUpType)
            {
                case PowerUpManager.PowerUpNames.TripleBlaster:
                case PowerUpManager.PowerUpNames.MultiBlaster:
                    Debug.Log("RocketFire - OnTriggerEnter2D - Change Audio for Triple and Multi");
                    audioSource.volume = 0.02f;
                    break;
                case PowerUpManager.PowerUpNames.SuperBlaster:
                    Debug.Log("RocketFire - OnTriggerEnter2D - Change Audio for Super");
                    audioSource.volume = 0.01f;
                    break;
            }

            Debug.Log("RocketFire - OnTriggerEnter2D - Before Play Audio");
            audioSource.Play();

        }
    }
}