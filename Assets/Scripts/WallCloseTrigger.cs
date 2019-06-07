using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCloseTrigger : MonoBehaviour
{
    public Collider2D CloseActionTriggerCollider;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(CloseActionTriggerCollider == collision)
        {
            Debug.Log($"OnTriggerExit2D: MATCH!");
            GameObject.Find("LevelBehavior23").GetComponent<LevelBehavior23>().StartWallCloseAnimation();
            Debug.Log($"BOOM!?");
        }
    }
}
