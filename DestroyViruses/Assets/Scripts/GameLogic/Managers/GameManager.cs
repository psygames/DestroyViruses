using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class GameManager : Singleton<GameManager>
    {
        void Start()
        {
            UIManager.Instance.loadViewFunc = LoadViewFunc;
            Application.logMessageReceived += LogCallback;
        }

        static ViewBase LoadViewFunc(string panelName)
        {
            if (panelName.Equals(typeof(LoadingView).Name))
            {
                return Resources.Load<ViewBase>("Prefabs/" + panelName);
            }
            return ResourceUtil.Load<ViewBase>(PathUtil.Panel(panelName));
        }

        static void LogCallback(string condition, string stackTrace, LogType type)
        {
            if (type >= LogType.Log)
            {
                Analytics.Event.Log(type, condition);
            }
        }
    }
}