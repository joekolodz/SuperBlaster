using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderPulse : MonoBehaviour
{
    [SerializeField][Range(0.0f, 2.0f)] private float Intensity = 0.0f;

    private SpriteRenderer _renderer;

    private float _timeElapsed;
    private float _lerpDuration = 0.5f;
    private float _lerpMaxValue = 2.0f;
    private float _lerpMinValue = 0.0f;
    private float _lerpTargetValue = 0.0f;
    private int _lerpDirection = 1;

    // Start is called before the first frame update
    public void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (_lerpDirection == 1)
        {
            _lerpTargetValue = _lerpMaxValue;
        }
        else
        {
            _lerpTargetValue = _lerpMinValue;
        }

        Intensity = Mathf.Lerp(Intensity, _lerpTargetValue, _timeElapsed / _lerpDuration);

        if (_timeElapsed < _lerpDuration)
        {
            _renderer.material.SetColor("_GlowColor", new Vector4(1.0f, 1.0f, 1.0f, 1f) * Intensity);
            _timeElapsed += Time.unscaledDeltaTime;
        }
        else
        {
            _lerpDirection *= -1;
            _timeElapsed = 0;
        }
    }
}
