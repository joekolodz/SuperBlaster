using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodGuyManager : MonoBehaviour
{
    public Transform guy;

    private void Start()
    {
        //guy = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(string.Format("Mouse: x{0}, y{1}", Input.mousePosition.x, Input.mousePosition.y));
        }

        var h = Input.GetAxisRaw("Horizontal");
        h *= 100;
        h *= Time.deltaTime;
        guy.transform.Rotate(0, 0, -h);
    }
}
