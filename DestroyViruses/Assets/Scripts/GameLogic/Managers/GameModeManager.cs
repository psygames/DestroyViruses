using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ReflectionEx;

namespace DestroyViruses
{
    public class GameModeManager : Singleton<GameModeManager>
    {
        public GameMode currentMode { get; private set; }

        private void OnApplicationPause(bool pause)
        {
            if (pause) Pause();
            else Resume();
        }

        public void InitMode<T>() where T : GameMode, new()
        {
            QuitMode();
            currentMode = new T();
            currentMode?.ReflectInvokeMethod("OnInit");
        }

        public void QuitMode()
        {
            currentMode?.ReflectInvokeMethod("OnQuit");
            currentMode = null;
        }

        public void Begin()
        {
            currentMode?.ReflectInvokeMethod("OnBegin");
        }

        public void End(bool isWin)
        {
            currentMode?.ReflectInvokeMethod("OnEnd", isWin);
        }

        public void Pause()
        {
            currentMode?.ReflectInvokeMethod("OnPause");
        }

        public void Resume()
        {
            currentMode?.ReflectInvokeMethod("OnResume");
        }

        void Update()
        {
            currentMode?.ReflectInvokeMethod("OnUpdate", Time.deltaTime);
        }
    }
}