using System;
using System.Collections.Generic;
using System.Reflection;

public class ProxyManager : Singleton<ProxyManager>
{
    private Dictionary<Type, object> mProxys = new Dictionary<Type, object>();

    private void subscribe<T>() where T : ProxyBase<T>, new()
    {
        var proxy = new T();
        InvokeMethod(proxy, "OnInit");
        mProxys.Add(typeof(T), proxy);
    }

    private void unsubscribe<T>() where T : ProxyBase<T>
    {
        var proxy = GetProxy<T>();
        if (proxy != null)
        {
            mProxys.Remove(typeof(T));
            InvokeMethod(proxy, "OnDestroy");
        }
    }

    private T getProxy<T>() where T : ProxyBase<T>
    {
        mProxys.TryGetValue(typeof(T), out object obj);
        return obj as T;
    }

    private void Update()
    {
        foreach (var kv in mProxys)
        {
            InvokeMethod(kv.Value, "OnUpdate");
        }
    }

    private void InvokeMethod(object obj, string method)
    {
        obj.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, new object[] { });
    }

    #region Scirpt APIs
    public static void Subscribe<T>() where T : ProxyBase<T>, new()
    {
        Instance.subscribe<T>();
    }

    public static void Unsubscribe<T>() where T : ProxyBase<T>
    {
        Instance.unsubscribe<T>();
    }

    public static T GetProxy<T>() where T : ProxyBase<T>
    {
        return Instance.getProxy<T>();
    }
    #endregion
}
