using UnityEngine;
using System;

public static class GameExtension
{
    public static float UpdateCD(this MonoBehaviour monoBehaviour, float value)
    {
        return Mathf.Max(0, value - Time.deltaTime);
    }
}
