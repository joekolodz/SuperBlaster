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
