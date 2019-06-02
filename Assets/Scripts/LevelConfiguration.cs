using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfiguration : MonoBehaviour
{
    public PowerUpManager.PowerUpNames PowerUpType;

    void Start()
    {
        switch(PowerUpType)
        {
            case PowerUpManager.PowerUpNames.SpeedBlaster:
                PowerUpManager.Instance.PowerUpSpeedBlasterForEntireLevel();
                break;
            case PowerUpManager.PowerUpNames.MultiBlaster:
                PowerUpManager.Instance.PowerUpMultiBlasterForEntireLevel();
                break;
            case PowerUpManager.PowerUpNames.TripleBlaster:
                PowerUpManager.Instance.PowerUpTripleBlasterForEntireLevel();
                break;
            case PowerUpManager.PowerUpNames.SuperBlaster:
                PowerUpManager.Instance.PowerUpSuperBlasterForEntireLevel();
                break;
        }
    }
}
