using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(ObjectHit))]
    internal class RadarShieldDestroy : MonoBehaviour
    {
        [SerializeField]
        private ObjectHit _shield;
        private bool _isDestroyedTriggered = false;

        void Reset()
        {
            _shield = gameObject.GetComponent<ObjectHit>();
        }

        void Update()
        {
            if (_isDestroyedTriggered == false && _shield.health <= 1)
            {
                _isDestroyedTriggered = true;
                EventAggregator.PublishRadarShieldDestroyed();
            }
        }
    }
}
