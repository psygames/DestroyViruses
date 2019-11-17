using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class GameManager : Singleton<GameManager>
    {
        static ViewBase LoadViewFunc(string panelName)
        {
            if (panelName.Equals(typeof(LoadingView).Name))
            {
                return Resources.Load<ViewBase>("Prefabs/" + panelName);
            }
            return ResourceUtil.Load<ViewBase>(PathUtil.Panel(panelName));
        }

        void Start()
        {
            UIManager.Instance.loadViewFunc = LoadViewFunc;
            ChangeState<SplashState>();
        }

        public static void ChangeState<T>() where T : StateMachine.State, new()
        {
            StateManager.ChangeState<T>();
        }
    }
}