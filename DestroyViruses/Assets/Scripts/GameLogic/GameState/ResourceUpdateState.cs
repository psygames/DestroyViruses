using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class ResourceUpdateState : StateBase
    {
        private AssetsUpdate assetsUpdate = new AssetsUpdate();

        public override void OnEnter()
        {
            assetsUpdate.Start();
            assetsUpdate.onError += (msg) => { Debug.LogError(msg); };
            // assetsUpdate.progress += (msg, percent) => { Debug.Log(msg + " => " + percent); };
            base.OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            assetsUpdate.Update();
            if (assetsUpdate.state == AssetsUpdate.State.Wait)
            {
                assetsUpdate.Check();
            }
            else if (assetsUpdate.state == AssetsUpdate.State.Completed)
            {
                GameManager.ChangeState<LoadingState>();
            }
        }
    }
}