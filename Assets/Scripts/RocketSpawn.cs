using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawn : MonoBehaviour
{
    public GameObject rocket;

    public Transform spawnPoint;

    private void Update()
    {
        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        bool isFiring = Input.GetButtonDown("Fire1");
        
        if (isFiring)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.y < -24) return; //don't fire when mouse is in menu area

            var guy = gameObject.GetComponent<GoodGuyManager>();
            if(guy.isSelected)
                Instantiate(rocket, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
