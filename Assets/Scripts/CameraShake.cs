using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _transform;//object to shake
    private float _shakeDuration = 0f;
    private float _shakeMagnitude = 0.3f;
    private float _dampingSpeed = 1.5f;// A measure of how quickly the shake effect should evaporate
    private Vector3 _initialPosition;

    private void Start()
    {
        EventAggregator.ShieldDestroyed += EventAggregator_ShieldDestroyed;
        EventAggregator.BaseDestroyed += HeavyShake;
        EventAggregator.PowerUpTriggered += HeavyShake;
    }

    public void OnDestroy()
    {
        EventAggregator.ShieldDestroyed -= EventAggregator_ShieldDestroyed;
        EventAggregator.BaseDestroyed -= HeavyShake;
        EventAggregator.PowerUpTriggered -= HeavyShake;
    }

    private void Awake()
    {
        if (_transform == null)
        {
            _transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable()
    {
        _initialPosition = _transform.localPosition;
    }

    private void Update()
    {
        if (_shakeDuration > 0)
        {
            var shakePosition = _initialPosition + Random.insideUnitSphere * _shakeMagnitude;
            _transform.position = new Vector3(shakePosition.x, shakePosition.y, _initialPosition.z);
            _shakeDuration -= Time.deltaTime * _dampingSpeed;
        }
        else
        {
            _shakeDuration = 0f;
            _transform.localPosition = _initialPosition;
        }
    }

    private void HeavyShake(object sender, System.EventArgs e)
    {
        _shakeMagnitude = 1.0f;
        _dampingSpeed = 1.0f;
        _shakeDuration = 1.0f;
    }


    private void EventAggregator_ShieldDestroyed(object sender, System.EventArgs e)
    {
        _shakeMagnitude = 0.28f;
        _dampingSpeed = 1.6f;
        _shakeDuration = 0.4f;
    }
}
