using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpawn : MonoBehaviour
{
    public GameObject plasma;
    public Transform spawnPoint;
    [Range(0, 10)]
    public int roundsPerBurst = 3;
    [Range(0, 1)]
    public float delayBetweenRounds = 0.07f;
    [Range(0, 10)]
    public float delayBetweenShots = 4.0f;

    private System.DateTime fireTime;
    private bool isFiring = false;

    void Start()
    {
        fireTime = System.DateTime.Now.AddSeconds(delayBetweenShots);
    }

    private void Update()
    {
        if (isFiring) return;

        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        if (plasma == null || spawnPoint == null) return;

        if (System.DateTime.Now > fireTime)
        {
            Fire();
            fireTime = System.DateTime.Now.AddSeconds(Random.Range(delayBetweenShots - 0.5f, delayBetweenShots + 0.5f));
        }
    }

    public void Fire()
    {
        StartCoroutine(FirePlasmaBurst());
    }

    private IEnumerator FirePlasmaBurst()
    {
        isFiring = true;

        for (var n = 1; n <= roundsPerBurst; n++)
        {
            if (plasma != null && spawnPoint != null)
            {
                Instantiate(plasma, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(delayBetweenRounds);
            }
            else
            {
                print(" !!! something went wrong, no plasma or spawnpoint");
            }
        }

        isFiring = false;
    }
}
