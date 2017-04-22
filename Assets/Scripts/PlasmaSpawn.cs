using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpawn : MonoBehaviour
{
    public GameObject plasma;
    public Transform spawnPoint;

    private void Update()
    {
        var isFiring = Input.GetButtonDown("Fire2");

        if (isFiring)
        {            
            StartCoroutine(FirePlasmaBurst());
        }
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
