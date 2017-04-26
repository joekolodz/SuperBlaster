using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : MonoBehaviour
{
    public List<GameObject> ShieldPosts;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<ObjectHit>().isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var post in ShieldPosts)
        {
            if (post != null) return;
        }

        gameObject.GetComponent<ObjectHit>().isEnabled = true;

    }
}
