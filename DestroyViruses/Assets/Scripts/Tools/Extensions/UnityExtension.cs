using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public static class UnityExtension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
            component = gameObject.AddComponent<T>();
        return component;
    }
}
