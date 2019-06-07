using UnityEngine;

public class ShieldState : MonoBehaviour
{
    private void OnDestroy()
    {
        EventAggregator.PublishShieldDestroyed();
    }
}
