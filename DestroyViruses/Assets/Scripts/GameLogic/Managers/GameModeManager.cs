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
        public bool isModeStarted { get { return mMode != null; } }
        private GameMode mMode = null;

        private void OnApplicationPause(bool pause)
        {
            if (pause) Pause();
            else Resume();
        }

        public void StartMode<T>() where T : GameMode, new()
        {
            Stop();
            mMode = new T();
            mMode?.ReflectInvokeMethod("OnInit");
        }

        public void Stop()
        {
            mMode?.ReflectInvokeMethod("OnQuit");
            mMode = null;
        }

        public void Pause()
        {
            mMode?.ReflectInvokeMethod("OnPause");
        }

        public void Resume()
        {
            mMode?.ReflectInvokeMethod("OnResume");
        }

        void Update()
        {
            mMode?.ReflectInvokeMethod("OnUpdate", Time.deltaTime);
        }
    }
}