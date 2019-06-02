using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHit : MonoBehaviour
{
    public GameObject smoke;
    public GameObject flames;

    [Range(0, 10)]
    public float smokeSizeMultiplier = 1;
    [Range(0, 10)]
    public float flameSizeMultiplier = 1;
    [Range(0, 50)]
    public int health = 3;
    public int healthWhenFlamesStart = 1;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains(hitTriggerObject.name))
        {
            if (PowerUpManager.Instance.IsPowerUp)
            {
                health -= PowerUpManager.Instance.GetAdjustedDamage();
            }
            else
            {
                health--;
            }

            if (!isSmoking)
            {
                smokeInstance = Instantiate(smoke, transform.position, Quaternion.identity);
                smokeInstance.transform.localScale *= smokeSizeMultiplier;

                var ps = smokeInstance.transform.Find("PS Smoke Trail");
                ps.localScale *= smokeSizeMultiplier;

                //say Yeah 30% of the time
                if (soundYeah != null && !soundYeah.isPlaying && Random.Range(0.0f, 1.0f) <= 0.3f)
                {
                    soundYeah.Play();
                }

                isSmoking = true;
            }
        }

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

        if (health <= 0)
        {
            var bx = gameObject.transform.parent.gameObject;
            var b = bx.GetComponent<BadGuyMovement>();
            if (b != null)
            {
                Returned++;
            }

            gameObject.GetComponent<ObjectDestroy>().Explode(delayDestroy);

        }
    }
    private int Returned = 0;

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
