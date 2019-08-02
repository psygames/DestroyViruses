using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class GameMode
    {
        protected virtual void OnInit() { }
        protected virtual void OnBegin() { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnEnd() { }
        protected virtual void OnQuit() { }
    }
}