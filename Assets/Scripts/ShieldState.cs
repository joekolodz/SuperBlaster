using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : MonoBehaviour
{
    public List<GameObject> ShieldPosts;

    // Use this for initialization
    void Start()
    {
        //this disabled the radar shield from taking damage as long as any shield post is still up
        //difficulty level - disable this for hard mode
        //gameObject.GetComponent<ObjectHit>().isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        return;
        foreach (var post in ShieldPosts)
        {
            if (post != null) return;
        }
        //only allow the radar shield (the parent of this component) to be hit when all shield posts are down
        gameObject.GetComponent<ObjectHit>().isEnabled = true;

    }
}
