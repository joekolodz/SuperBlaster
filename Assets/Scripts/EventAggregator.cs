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

public class ObjectDestroyedEventArgs : EventArgs
{
    public Transform Transform;
    public float ExplosionScale = 1.0f;

    public ObjectDestroyedEventArgs(Transform transform)
    {
        Transform = transform;
    }

    public ObjectDestroyedEventArgs(Transform transform, float explosionScale)
    {
        Transform = transform;
        ExplosionScale = explosionScale;
    }
}

public class EventAggregator
{
    public static event EventHandler PowerUpExpired;
    public static event EventHandler<PowerUpTriggeredEventArgs> PowerUpTriggered;
    public static event EventHandler<ObjectDestroyedEventArgs> ObjectDestroyed;

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
}
