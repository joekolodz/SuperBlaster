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
        var h = Input.GetAxisRaw("Horizontal");
        h *= 100;
        h *= Time.deltaTime;
        guy.transform.Rotate(0, 0, -h);
    }
}
