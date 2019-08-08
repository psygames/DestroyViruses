using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
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

        public void StartMode<T>() where T : GameMode, new()
        {
            Stop();
            currentMode = new T();
            currentMode?.ReflectInvokeMethod("OnInit");
        }

        public void Stop()
        {
            currentMode?.ReflectInvokeMethod("OnQuit");
            currentMode = null;
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