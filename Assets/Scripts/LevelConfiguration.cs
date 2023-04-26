using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfiguration : MonoBehaviour
{
    public PowerUpManager.PowerUpNames PowerUpType;

    void Start()
    {
        PowerUpManager.Instance.ForcePowerUp(PowerUpType);
    }
}
