using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class ResourceUpdateState : StateBase
    {
        private AssetsUpdate updater = new AssetsUpdate();

        public float progress { get; private set; }
        public string message { get; private set; }

        public override void OnEnter()
        {
            progress = 0;
            updater.onError += (msg) => { Debug.LogError(msg); };
            updater.Init();
            base.OnEnter();
        }

        private void UpdateMessage()
        {
            if (updater.state == AssetsUpdate.State.Wait)
            {
                message = "资源热更新...";
                progress = 0.1f;
            }
            else if (updater.state == AssetsUpdate.State.Checking)
            {
                message = "检查需要更新的资源...";
                progress = 0.2f;
            }
            else if (updater.state == AssetsUpdate.State.Downloading)
            {
                message = $"下载资源：[{updater.downloadIndex}/{updater.downloadCount}]";
                progress = updater.progress * 0.8f + 0.2f;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            updater.Update();
            UpdateMessage();

            if (updater.state == AssetsUpdate.State.Wait)
            {
                Debug.Log("check resource update");
                UIManager.Instance.Open<LoadingPanel>();
                updater.Check();
            }
            else if (updater.state == AssetsUpdate.State.Completed)
            {
                GameManager.ChangeState<LoadingState>();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            UIManager.Instance.Close<LoadingPanel>();
        }
    }
}