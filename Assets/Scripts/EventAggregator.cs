using System;

public class PowerUpTriggeredEventArgs : EventArgs
{
    public PowerUpManager.PowerUpNames PowerUpName;

    public PowerUpTriggeredEventArgs(PowerUpManager.PowerUpNames powerUpName)
    {
        PowerUpName = powerUpName;
    }
}

public class EventAggregator
{
    public static event EventHandler PowerUpExpired;
    public static event EventHandler<PowerUpTriggeredEventArgs> PowerUpTriggered;

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
}
