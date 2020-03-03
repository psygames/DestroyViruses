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
            var isAssetsInited = false;
            Plugins.XAsset.Assets.Initialize(() => { isAssetsInited = true; }, (str) => Debug.LogError(str));
            yield return null;
            while (!isAssetsInited)
                yield return null;
            Debug.Log("XAsset Initialized");

#if PUBLISH_BUILD
            StateManager.ChangeState<LoadingState>();
#else
            StateManager.ChangeState<HotUpdateState>();
#endif
        }
    }
}