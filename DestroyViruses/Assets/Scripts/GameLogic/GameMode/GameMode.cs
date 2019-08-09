using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class GameMode
    {
        public bool isInit { get; private set; }
        public bool isPause { get; private set; }
        public bool isBegin { get; private set; }
        public float runningTime { get; private set; }

        protected virtual void OnInit()
        {
            isInit = true;
        }
        protected virtual void OnQuit()
        {
            isInit = false;
        }

        protected virtual void OnBegin()
        {
            runningTime = 0;
            isBegin = true;
        }
        protected virtual void OnEnd(bool isWin)
        {
            runningTime = 0;
            isBegin = false;
        }
        protected virtual void OnPause()
        {
            isPause = true;
        }
        protected virtual void OnResume()
        {
            isPause = false;
        }
        protected virtual void OnUpdate(float deltaTime)
        {
            if (isInit && isBegin && !isPause)
            {
                runningTime += deltaTime;
            }
        }
    }
}