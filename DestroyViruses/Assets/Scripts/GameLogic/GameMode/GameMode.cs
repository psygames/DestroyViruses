using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class GameMode
    {
        public bool isInit { get; private set; }
        public bool isRunning { get; private set; }
        public bool isBegin { get; private set; }
        public float runningTime { get; private set; }

        protected virtual void OnInit()
        {
            isInit = true;
            isBegin = false;
            isRunning = false;
            runningTime = 0;
        }

        protected virtual void OnQuit()
        {
            isInit = false;
            isBegin = false;
            isRunning = false;
        }

        protected virtual void OnBegin()
        {
            runningTime = 0;
            isBegin = true;
            isRunning = true;
        }

        protected virtual void OnEnd(bool isWin)
        {
            isBegin = false;
            isRunning = false;
        }

        protected virtual void OnPause()
        {
            isRunning = false;
        }

        protected virtual void OnResume()
        {
            isRunning = true;
        }

        protected virtual void OnUpdate(float deltaTime)
        {
            if (isInit && isBegin && isRunning)
            {
                runningTime += deltaTime;
            }
        }
    }
}