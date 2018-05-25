using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RocketSpawn : MonoBehaviour
{
    public GameObject rocket;

    public Transform spawnPoint;

    private void Update()
    {
        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        bool isFiring = false;// Input.GetButtonDown("Fire1");

        if (isFiring)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.y < -24) return; //don't fire when mouse is in menu area

            var guy = gameObject.GetComponent<GoodGuyManager>();
            if (guy.isSelected)
            {
                //need to get the angle from the rocket launcher since it is above center of the guy (which would cause a bad angle if we used guy's position)
                //var rocketLauncher2 = gameObject.GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(t => t.name == "Rocket Launcher");
                var rocketLauncher = gameObject.transform.Find("Good Guy/Rocket Launcher");


                //first, turn guy to face mouse click point (using rocket position instead of guy position)
                if (guy.guy.rotation.eulerAngles.y == 180)
                {
                    //call reverse since the guys on the right are flipped 180
                    guy.transform.rotation = GeometricFunctions.RotateToFace(mousePos, rocketLauncher.transform.position);
                }
                else
                {
                    guy.transform.rotation = GeometricFunctions.RotateToFace(rocketLauncher.transform.position, mousePos);
                }

                //then instantiate rocket
                Instantiate(rocket, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
