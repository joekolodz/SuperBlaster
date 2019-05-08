using System;
using System.Collections;
using UnityEngine;

public class WaitForTime
{
    public static IEnumerator Wait(float seconds, Action method)
    {
        yield return new WaitForSeconds(seconds);
        method.Invoke();
        yield return null;
    }
}
