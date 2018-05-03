using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodGuyManager : MonoBehaviour
{
    public Transform guy;
    public bool isSelected = false;

    private void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");

        var guy = gameObject.GetComponent<GoodGuyManager>();
        if (guy.isSelected)
        {
            h *= 100;
            h *= Time.deltaTime;
            guy.transform.Rotate(0, 0, -h);
        }
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
