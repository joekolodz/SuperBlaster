using UnityEngine;

public class LaserFire : MonoBehaviour
{

    public float Force = 10 * 1000;
    public float TimeToLiveInSeconds = 0.25f;

    private float _expirationTime;

    private void Awake()
    {
        _expirationTime = Time.time + TimeToLiveInSeconds;        
    }
}
