using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class SplashState : StateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            ProxyManager.Subscribe<InternalProxy>();
            GameManager.Instance.StartCoroutine(TaskBegin());
        }

        private IEnumerator TaskBegin()
        {
            yield return null;
            StateManager.ChangeState<LoadingState>();
        }
    }
}