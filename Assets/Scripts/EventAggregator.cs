using UnityEngine;
using System;

public class PowerUpTriggeredEventArgs : EventArgs
{
    public PowerUpManager.PowerUpNames PowerUpName;

    public PowerUpTriggeredEventArgs(PowerUpManager.PowerUpNames powerUpName)
    {
        PowerUpName = powerUpName;
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

public class EventAggregator
{
    public static event EventHandler PowerUpExpired;
    public static event EventHandler<PowerUpTriggeredEventArgs> PowerUpTriggered;
    public static event EventHandler<ObjectDestroyedEventArgs> ObjectDestroyed;
    public static event EventHandler<BadGuyDiedEventArgs> BadGuyDied;
    public static event EventHandler AllBadGuysKilled;
    public static event EventHandler PlasmaBlastHit;
    public static event EventHandler PlasmaBlastFired;
    public static event Action<EventArgs> whatever;

    protected static void OnPowerUpExpired(EventArgs e)
    {
        var handler = PowerUpExpired;
        handler?.Invoke(null, e);
    }

    public static void PublishPowerUpExpired()
    {
        OnPowerUpExpired(new EventArgs());
    }


    protected static void OnPowerUpTriggered(PowerUpTriggeredEventArgs e)
    {
        var handler = PowerUpTriggered;
        handler?.Invoke(null, e);
    }

    public static void PublishPowerUpTriggered(PowerUpTriggeredEventArgs e)
    {
        OnPowerUpTriggered(e);
    }


    protected static void OnObjectDestroyed(ObjectDestroyedEventArgs e)
    {
        var handler = ObjectDestroyed;
        handler?.Invoke(null, e);
    }

    public static void PublishObjectDestroyed(ObjectDestroyedEventArgs e)
    {
        OnObjectDestroyed(e);
    }

    protected static void OnBadGuyDied(BadGuyDiedEventArgs e)
    {
        var handler = BadGuyDied;
        handler?.Invoke(null, e);
    }


    public static void PublishBadGuyDied(BadGuyDiedEventArgs e)
    {
        OnBadGuyDied(e);
    }

    protected static void OnAllBadGuysKilled(EventArgs e)
    {
        var handler = AllBadGuysKilled;
        handler?.Invoke(null, e);
    }

    public static void PublishAllBadGuysKilled()
    {
        OnAllBadGuysKilled(new EventArgs());
    }

    protected static void OnPlasmaBlastHit(EventArgs e)
    {
        var handler = PlasmaBlastHit;
        handler?.Invoke(null, e);
    }

    public static void PublishPlasmaBlastHit()
    {
        OnPlasmaBlastHit(new EventArgs());
    }

    protected static void OnPlasmaBlastFired(EventArgs e)
    {
        var handler = PlasmaBlastFired;
        handler?.Invoke(null, e);
    }

    public static void PublishPlasmaBlastFired()
    {
        OnPlasmaBlastFired(new EventArgs());
    }

}
