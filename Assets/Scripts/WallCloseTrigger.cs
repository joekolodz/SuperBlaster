using UnityEngine;

public class WallCloseTrigger : MonoBehaviour
{
    public Collider2D CloseActionTriggerCollider;
    public int WallId;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(CloseActionTriggerCollider == collision)
        {
            EventAggregator.PublishWallCloseTriggered(new WallCloseTriggeredEventArgs(WallId));
        }
    }
}
