using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public abstract class GameMode
    {
        protected abstract void OnInit();
        protected abstract void OnBegin();
        protected abstract void OnPause();
        protected abstract void OnResume();
        protected abstract void OnUpdate(float deltaTime);
        protected abstract void OnEnd();
        protected abstract void OnQuit();
    }
}