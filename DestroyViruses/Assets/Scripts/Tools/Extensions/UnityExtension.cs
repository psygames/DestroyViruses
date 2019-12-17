using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public static class UnityExtension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
            component = gameObject.AddComponent<T>();
        return component;
    }

    public static void DelayDo(this MonoBehaviour mono, float delay, Action callback)
    {
        mono.StartCoroutine(DelayDoCoro(delay, callback));
    }

    private static IEnumerator DelayDoCoro(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
