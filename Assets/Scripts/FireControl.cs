using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public Transform[] _goodGuys;
    public GoodGuyManager _currentlySelectedGuy;

    public void Update()
    {
        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        bool isFiring = Input.GetButtonDown("Fire1");

        if (isFiring)
        {
            //are we clicking on something?
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var guy = IsGuySelected(mousePos);
            if (guy)
            {
                SelectGuy(guy);
            }
            else
            {
                FireARocket(_currentlySelectedGuy, mousePos);
            }
        }
    }

    private GoodGuyManager IsGuySelected(Vector3 mousePos)
    {
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);//use V2 because we don't want the Z axis
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); //zero means straight down under the mousepoint

        if (hit.collider != null)
        {
            //can we find the selected guy?
            return hit.collider.gameObject.GetComponent<GoodGuyManager>();
        }
        return null;
    }

    void FireARocket(GoodGuyManager guyFiring, Vector3 mousePos)
    {
        if (mousePos.y < -24) return; //don't fire when mouse is in menu area

        if (guyFiring.isSelected)
        {
            //need to get the angle from the rocket launcher since it is above center of the guy (which would cause a bad angle if we used guy's position)
            //var rocketLauncher2 = gameObject.GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(t => t.name == "Rocket Launcher");
            var rocketLauncher = guyFiring.transform.Find("Good Guy/Rocket Launcher");


            //first, turn guy to face mouse click point (using rocket position instead of guy position)
            if (guyFiring.guy.rotation.eulerAngles.y == 180)
            {
                //call reverse since the guys on the right are flipped 180
                guyFiring.transform.rotation = GeometricFunctions.RotateToFace(mousePos, rocketLauncher.transform.position);
            }
            else
            {
                guyFiring.transform.rotation = GeometricFunctions.RotateToFace(rocketLauncher.transform.position, mousePos);
            }
            
            var rocketSpawn = guyFiring.GetComponent<RocketSpawn>();
            //then instantiate rocket
            Instantiate(rocketSpawn.rocket, rocketSpawn.spawnPoint.position, rocketSpawn.spawnPoint.rotation);
        }
    }

    // Use this for initialization
    void Start()
    {
        //sets the Top Left good guy as the default selected
        _goodGuys[0].GetComponent<GoodGuyManager>().Select();
    }

    public void UnselectAllGuys()
    {
        foreach (var guy in _goodGuys)
        {
            guy.GetComponent<GoodGuyManager>().Unselect();
        }
    }

    public void SelectGuy(GoodGuyManager guy)
    {
        UnselectAllGuys();
        guy.Select();
        _currentlySelectedGuy = guy;
    }
}
