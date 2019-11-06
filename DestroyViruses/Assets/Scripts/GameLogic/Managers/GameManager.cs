using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class GameManager : Singleton<GameManager>
    {
        static PanelBase LoadPanelFunc(string panelName)
        {
            if (panelName.Equals(typeof(LoadingPanel).Name))
            {
                return Resources.Load<PanelBase>("Prefabs/" + panelName);
            }
            return ResourceUtil.Load<PanelBase>(PathUtil.Panel(panelName));
        }

        void Start()
        {
            UIManager.Instance.loadPanelFunc = LoadPanelFunc;
            ChangeState<SplashState>();
        }

        public static void ChangeState<T>() where T : StateMachine.State, new()
        {
            StateManager.ChangeState<T>();
        }
    }
}