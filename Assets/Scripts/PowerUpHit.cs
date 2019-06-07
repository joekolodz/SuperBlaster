using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHit : MonoBehaviour
{
    /// <summary>
    /// The thing that destroyed this object
    /// </summary>
    public GameObject hitTriggerObject;
    public float delayDestroy = 0f;//0.8f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains(hitTriggerObject.name))
        {
            gameObject.GetComponent<ObjectDestroy>().Explode(delayDestroy);
        }
    }
}