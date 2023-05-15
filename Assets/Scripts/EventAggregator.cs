using UnityEngine;
using System;
using UnityEngine.Events;

public class PowerUpTriggeredEventArgs : EventArgs
{
    public PowerUpManager.PowerUpNames PowerUpName;

    public PowerUpTriggeredEventArgs(PowerUpManager.PowerUpNames powerUpName)
    {
        PowerUpName = powerUpName;
    }
}

public class PowerDownTriggeredEventArgs : EventArgs
{
    public PowerDownManager.PowerDownNames PowerDownName;

    public PowerDownTriggeredEventArgs(PowerDownManager.PowerDownNames powerDownName)
    {
        PowerDownName = powerDownName;
    }
}

public class BadGuyDiedEventArgs : EventArgs
{
    public GameObject BadGuy;
    public int DestructionValue;

    public BadGuyDiedEventArgs(GameObject deadBadGuy, int destructionValue)
    {
        BadGuy = deadBadGuy;
        DestructionValue = destructionValue;
    }
}

public class ObjectDestroyedEventArgs : EventArgs
{
    public Transform Transform;
    public float ExplosionScale = 1.0f;
    public bool IsSmallExplosion = true;

    public ObjectDestroyedEventArgs(Transform transform)
    {
        Transform = transform;
    }

    public ObjectDestroyedEventArgs(Transform transform, float explosionScale)
    {
        Transform = transform;
        ExplosionScale = explosionScale;
    }

    public ObjectDestroyedEventArgs(Transform transform, float explosionScale, bool isSmallExplosion)
    {
        Transform = transform;
        ExplosionScale = explosionScale;
        IsSmallExplosion = isSmallExplosion;
    }
}

public class WallCloseTriggeredEventArgs : EventArgs
{
    public int WallId;

    public WallCloseTriggeredEventArgs(int wallId)
    {
        WallId = wallId;
    }
}

public class EventAggregator
{
    public static event EventHandler PowerUpExpired;
    public static event EventHandler<PowerUpTriggeredEventArgs> PowerUpTriggered;
    public static event EventHandler PowerDownExpired;
    public static event EventHandler<PowerDownTriggeredEventArgs> PowerDownTriggered;
    public static event EventHandler<ObjectDestroyedEventArgs> ObjectDestroyed;
    public static event EventHandler<BadGuyDiedEventArgs> BadGuyDied;
    public static event EventHandler LevelCompleted;
    public static event EventHandler PlasmaBlastHit;
    public static event EventHandler PlasmaBlastFired;
    public static event EventHandler ShieldDestroyed;
    public static event EventHandler BaseDestroyed;
    public static event EventHandler<WallCloseTriggeredEventArgs> WallCloseTriggered;
    public static event EventHandler AbortLevel;

    public static void PublishPowerUpExpired()
    {
        PowerUpExpired?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishPowerUpTriggered(PowerUpTriggeredEventArgs e)
    {
        PowerUpTriggered?.Invoke(null, e);
    }

    public static void PublishPowerDownExpired()
    {
        PowerUpExpired?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishPowerDownTriggered(PowerDownTriggeredEventArgs e)
    {
        PowerDownTriggered?.Invoke(null, e);
    }

    public static void PublishObjectDestroyed(ObjectDestroyedEventArgs e)
    {
        ObjectDestroyed?.Invoke(null, e);
    }

    public static void PublishBadGuyDied(BadGuyDiedEventArgs e)
    {
        BadGuyDied?.Invoke(null, e);
    }

    public static void PublishLevelCompleted()
    {
        LevelCompleted?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishPlasmaBlastHit()
    {
        PlasmaBlastHit?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishPlasmaBlastFired()
    {
        PlasmaBlastFired?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishShieldDestroyed()
    {
        ShieldDestroyed?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishBaseDestroyed()
    {
        BaseDestroyed?.Invoke(null, EventArgs.Empty);
    }

    public static void PublishWallCloseTriggered(WallCloseTriggeredEventArgs e)
    {
        WallCloseTriggered?.Invoke(null, e);
    }

    public static void PublishAbortLevel()
    {
        AbortLevel?.Invoke(null, EventArgs.Empty);
    }

}
