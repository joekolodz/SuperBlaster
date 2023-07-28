using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts
{
    internal class LightsOut : MonoBehaviour
    {
        void Awake()
        {
            EventAggregator.RadarShieldDestroyed += EventAggregator_RadarShieldDestroyed;
        }

        private void EventAggregator_RadarShieldDestroyed(object sender, EventArgs e)
        {
            var mainLight = gameObject.GetComponent<Light2D>();
            mainLight.intensity = 0.3f;
        }

        void OnDestroy()
        {
            EventAggregator.RadarShieldDestroyed -= EventAggregator_RadarShieldDestroyed;
        }
    }
}
