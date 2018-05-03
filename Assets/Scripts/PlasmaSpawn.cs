using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpawn : MonoBehaviour
{
    public GameObject plasma;
    public Transform spawnPoint;
    [Range(0, 10)]
    public int roundsPerBurst = 3;
    [Range(0, 2)]
    public float delayBetweenRounds = 0.07f;

    private System.DateTime fireTime;

    void Start()
    {
        fireTime = System.DateTime.Now.AddSeconds(3);
    }

    private void Update()
    {
        if (plasma == null || spawnPoint == null) return;

        if (System.DateTime.Now > fireTime)
        {
            Fire();
            fireTime = System.DateTime.Now.AddSeconds(Random.Range(2, 4));
        }


        var isFiring = Input.GetButtonDown("Fire2");

        if (isFiring)
        {
            Fire();
        }
    }

    public void Fire()
    {
        StartCoroutine(FirePlasmaBurst());
    }

    private IEnumerator FirePlasmaBurst()
    {
        for (var n = 0; n <= roundsPerBurst; n++)
        {
            if (plasma != null && spawnPoint != null)
            {
                Instantiate(plasma, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSeconds(delayBetweenRounds);
            }
        }
    }
}
