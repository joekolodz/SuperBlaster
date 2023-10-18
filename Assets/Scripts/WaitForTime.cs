using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class WaitForTime
{
    public static IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return null;
    }

    public static IEnumerator Wait(float seconds, Action method)
    {
        yield return new WaitForSeconds(seconds);
        method.Invoke();
        yield return null;
    }
    public static IEnumerator WaitUnscaled(float seconds, Action method)
    {
        var elapsed = 0.0f;
        while (elapsed < seconds)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        method.Invoke();
    }
}
