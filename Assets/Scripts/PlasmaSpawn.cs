using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpawn : MonoBehaviour
{
    public GameObject plasma;
    public Transform spawnPoint;

    private System.DateTime fireTime;

    void Start()
    {
        fireTime = System.DateTime.Now.AddSeconds(3);
    }

    private void Update()
    {
        if (plasma == null || spawnPoint == null) return;

        if(System.DateTime.Now > fireTime)
        {
            Fire();
            fireTime = System.DateTime.Now.AddSeconds(Random.Range(2,4));
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
        Instantiate(plasma, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(.07f);
        Instantiate(plasma, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(.07f);
        Instantiate(plasma, spawnPoint.position, spawnPoint.rotation);
    }
}
