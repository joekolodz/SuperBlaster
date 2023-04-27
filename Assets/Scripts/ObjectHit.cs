using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    public bool IsIndestructable = false;
    public GameObject smoke;
    public GameObject flames;

    [Range(0, 10)]
    public float smokeSizeMultiplier = 1;
    [Range(0, 10)]
    public float flameSizeMultiplier = 1;
    [Range(0, 200)]
    public int health = 3;
    public int healthWhenFlamesStart = 1;
    public int healthWhenSmokeStarts = 0;
    /// <summary>
    /// The thing that destroyed this object
    /// </summary>
    public GameObject hitTriggerObject;
    public float delayDestroy = 0f;//0.8f;

    public AudioSource soundYeah;

    private GameObject smokeInstance;
    private bool isSmoking = false;

    private GameObject flamesInstance;
    private bool isOnFire = false;

    private void Start()
    {
        if (healthWhenSmokeStarts == 0)
        {
            healthWhenSmokeStarts = health - 1;
        }
    }

    private void IsSmoking()
    {
        if (!isSmoking)
        {
            if (health <= healthWhenSmokeStarts)
            {
                smokeInstance = Instantiate(smoke, transform.position, Quaternion.identity);
                smokeInstance.transform.localScale *= smokeSizeMultiplier;
                var ps = smokeInstance.transform.Find("PS Smoke Trail");
                ps.localScale *= smokeSizeMultiplier;
                isSmoking = true;
            }

            if (soundYeah != null && !soundYeah.isPlaying && Random.Range(0.0f, 1.0f) <= 0.2f)
            {
                soundYeah.volume = 0.1f;
                soundYeah.Play();
            }

        }
    }

    private void IsOnFire()
    {
        if (health <= healthWhenFlamesStart)
        {
            if (!isOnFire && flames != null)
            {
                flamesInstance = Instantiate(flames, transform.position, Quaternion.identity);
                flamesInstance.transform.localScale *= flameSizeMultiplier;

                var ps = flamesInstance.transform.Find("PS Flame Sparks");
                ps.localScale *= flameSizeMultiplier;

                isOnFire = true;
            }
        }
    }

    private void IsKilled()
    {
        if (health <= 0)
        {
            //TODO get gameObject's death animation (so walls crumble etc)
            gameObject.GetComponent<ObjectDestroy>().Explode(delayDestroy);
        }
    }

    private void UpdateDamageState()
    {
        IsSmoking();
        IsOnFire();
        IsKilled();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectHit(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectHit(collision.gameObject);
    }

    private void DetectHit(GameObject collidingGameObject)
    {
        if (hitTriggerObject == null) return;
        if (collidingGameObject.name == hitTriggerObject.name ||
            collidingGameObject.name == hitTriggerObject.name + "(Clone)")
        {
            TakeDamage(PowerUpManager.Instance.IsPowerUp ? PowerUpManager.Instance.GetAdjustedDamage() : 1);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (IsIndestructable) return;
        health -= damageAmount;
        UpdateDamageState();
    }

    public void Update()
    {
        if (smokeInstance != null)
        {
            smokeInstance.transform.position = gameObject.transform.position;
        }

        if (flamesInstance != null)
        {
            flamesInstance.transform.position = gameObject.transform.position;
        }
    }

    private void OnDestroy()
    {
        if (smokeInstance != null)
            Destroy(smokeInstance);

        if (flamesInstance != null)
            Destroy(flamesInstance);
    }

    protected void OnDisable()
    {
        OnDestroy();
    }
}
