using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class GoodGuyManager : MonoBehaviour
{
    public Transform guy;
    public bool isSelected = false;
    public bool isSelectedOnStartup = false;

    private void Awake()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var guy = gameObject.GetComponent<GoodGuyManager>();
        if (guy.isSelected)
        {
            if (h != 0.0f)
            {
                h = ApplyRotation(h, guy);
            }
            else
            {
                if (v != 0.0f)
                {
                    v = ApplyRotation(-v, guy);
                }
            }
        }
    }

    public RocketSpawn AimAtMouse(Vector3 mousePos)
    {
        //need to get the angle from the rocket launcher since it is above center of the guy (which would cause a bad angle if we used guy's position)
        var rocketLauncher = transform.Find("Good Guy/Rocket Launcher");

        //first, turn guy to face mouse click point
        if (guy.rotation.eulerAngles.y == 180)
        {
            //call reverse since the guys on the right are flipped 180
            transform.rotation = GeometricFunctions.RotateToFace(mousePos, rocketLauncher.transform.position);
        }
        else
        {
            transform.rotation = GeometricFunctions.RotateToFace(rocketLauncher.transform.position, mousePos);
        }

        return GetComponent<RocketSpawn>();
    }

    private static float ApplyRotation(float amount, GoodGuyManager guy)
    {
        amount *= 100;
        amount *= Time.deltaTime;
        guy.transform.Rotate(0, 0, -amount);
        return amount;
    }

    public void Select()
    {
        isSelected = true;
        guy.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void Unselect()
    {
        isSelected = false;
        guy.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
