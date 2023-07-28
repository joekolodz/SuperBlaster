using UnityEngine;

public class BaseDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject _alarmLights;

    private Animator _animatorAlarmLights;

    void Start()
    {
        EventAggregator.RadarShieldDestroyed += EventAggregator_RadarShieldDestroyed;
    }

    private void EventAggregator_RadarShieldDestroyed(object sender, System.EventArgs e)
    {
        if (_alarmLights == null)
        {
            return;
        }

        _animatorAlarmLights = _alarmLights.GetComponent<Animator>();
        _alarmLights.SetActive(true);
        _animatorAlarmLights.Play("Alarm Lights");
    }

    void OnDisable()
    {
        if (_animatorAlarmLights != null)
        {
            _animatorAlarmLights.enabled = false;
        }
        _alarmLights.SetActive(false);
    }

    void OnDestroy()
    {
        if (StateManager.isWaitingForNextLevelToStart) return;
        EventAggregator.RadarShieldDestroyed -= EventAggregator_RadarShieldDestroyed;
        EventAggregator.PublishBaseDestroyed();
    }
}
