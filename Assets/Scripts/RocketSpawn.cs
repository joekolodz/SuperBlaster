using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawn : MonoBehaviour
{
    public GameObject rocket;

    public Transform spawnPoint;

    private void Update()
    {
        bool isFiring = Input.GetButtonDown("Fire1");
        

        if (isFiring)
            Instantiate(rocket, spawnPoint.position, spawnPoint.rotation);
    }
}
