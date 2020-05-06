using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class GameModeManager : Singleton<GameModeManager>
{
    public GameModeBase currentMode { get; private set; }

    private void OnApplicationPause(bool pause)
    {
        if (pause) Pause();
        else Resume();
    }

    public void InitMode<T>() where T : GameModeBase, new()
    {
        QuitMode();
        currentMode = new T();
        ReflectInvokeMethod(currentMode, "OnInit");
    }

    public void QuitMode()
    {
        ReflectInvokeMethod(currentMode, "OnQuit");
        currentMode = null;
    }

    public void Begin()
    {
        ReflectInvokeMethod(currentMode, "OnBegin");

    }

    public void End(bool isWin)
    {
        ReflectInvokeMethod(currentMode, "OnEnd", isWin);
    }

    public void Pause()
    {
        ReflectInvokeMethod(currentMode, "OnPause");
    }

    public void Resume()
    {
        ReflectInvokeMethod(currentMode, "OnResume");
    }

    void Update()
    {
        ReflectInvokeMethod(currentMode, "OnUpdate", Time.deltaTime);
    }

    private void ReflectInvokeMethod(object obj, string name, params object[] parameters)
    {
        if (obj == null)
        {
            return;
        }

        BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var m = obj.GetType().GetMethod(name, flags);
        if (m == null)
        {
            Debug.LogError($"{obj.GetType().Name} no Method named {name}.");
            return;
        }
        m.Invoke(obj, parameters);
    }
}
