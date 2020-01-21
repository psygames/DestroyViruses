using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class LoadingState : StateBase
    {
        public float progress { get; private set; }
        public string message { get; private set; }
        private float step = 0.1f;
        private bool taskFinished;

        public override void OnEnter()
        {
            base.OnEnter();
            TaskBegin();
        }

        private void TaskBegin()
        {
            taskFinished = false;
            GameManager.Instance.StartCoroutine(TaskAll());
        }

        private void TaskFinish()
        {
            taskFinished = true;
        }

        private Dictionary<string, float> mTaskWait = new Dictionary<string, float>();
        private bool WaitTimeout(string key, float timeout = 3, bool logError = true)
        {
            if (!mTaskWait.ContainsKey(key))
            {
                mTaskWait.Add(key, Time.time);
                return false;
            }
            bool isTimeout = Time.time - mTaskWait[key] > timeout;
            if (isTimeout)
            {
                if (logError)
                    Debug.LogError($"{key} Timeout.");
                else
                    Debug.LogWarning($"{key} Timeout.");
            }
            return isTimeout;
        }

        IEnumerator TaskAll()
        {
            mTaskWait.Clear();

            // #0 Delay One Frame
            yield return null;

            // #0.5 Remote Config
            progress += step;
            message = LTKey.LOADING_INITIALIZE_REMOTE_CONFIG.LT();
            yield return null;
            ProxyManager.Subscribe<RemoteConfigProxy>();
            yield return null;
            while (!RemoteConfigProxy.Ins.isInit
                && !WaitTimeout("RemoteConfig", 0.1f, false))
                yield return null;

            // #1
            progress += step;
            message = LTKey.LOADING_INITIALIZE_DATA.LT();
            yield return null;
            ProxyManager.Subscribe<DataProxy>();
            yield return null;

            // #2
            progress += step;
            message = LTKey.LOADING_INITIALIZE_ANALYTICS.LT();
            yield return null;
            ProxyManager.Subscribe<AnalyticsProxy>();
            yield return null;
            while (!AnalyticsProxy.Ins.isInit
                && !AnalyticsProxy.Ins.isInitFailed
                && !WaitTimeout("Analytics", 0.1f, false))
                yield return null;

            // #3
            progress += step;
            message = LTKey.LOADING_INITIALIZE_ADVERTISEMENT.LT();
            yield return null;
            ProxyManager.Subscribe<AdProxy>();
            yield return null;
            while (!AdProxy.Ins.isInit
                && !WaitTimeout("Advertisement", 0.1f, false))
                yield return null;

            // #4
            progress += step;
            message = LTKey.LOADING_INITIALIZE_IN_APP_PURCHASE.LT();
            yield return null;
            IAPManager.Instance.Init();
            yield return null;
            while (!IAPManager.Instance.isInit
                && !IAPManager.Instance.isInitFailed
                && !WaitTimeout("In-App Purchase", 0.1f, false))
                yield return null;

            // #5
            progress += step;
            message = LTKey.LOADING_INITIALIZE_SETTINGS.LT();
            yield return null;
            Application.targetFrameRate = ConstTable.table.frameRate;
            yield return null;

            // #6
            progress += step;

            message = LTKey.LOADING_INITIALIZE_RESOURCES_ATLAS.LT();
            yield return null;
            UIUtil.LoadAtlasAll();
            yield return null;

            message = LTKey.LOADING_INITIALIZE_RESOURCES_ENTITIES.LT();
            yield return null;
            EntityManager.WarmPoolAll();
            yield return null;

            message = LTKey.LOADING_INITIALIZE_RESOURCES_VIEWS.LT();
            yield return null;
            UIManager.Load<BattleView>();
            yield return null;

            message = LTKey.LOADING_INITIALIZE_RESOURCES_SOUNDS.LT();
            AudioManager.Preload("BGM1");
            yield return null;
            AudioManager.Preload("BGM2");
            yield return null;
            AudioManager.Preload("BGM3");
            yield return null;
            AudioManager.Preload("BGM4");
            yield return null;
            AudioManager.Preload("hit");
            yield return null;

            message = LTKey.LOADING_INITIALIZE_RESOURCES_TABLES.LT();
            TableAdsCollection.Instance.GetAll();
            TableAircraftCollection.Instance.GetAll();
            yield return null;
            TableBuffCollection.Instance.GetAll();
            TableBuffAutoGenCollection.Instance.GetAll();
            yield return null;
            TableBuffKillGenCollection.Instance.GetAll();
            TableCoinIncomeCollection.Instance.GetAll();
            yield return null;
            TableCoinValueCollection.Instance.GetAll();
            TableConstCollection.Instance.GetAll();
            yield return null;
            TableDailySignCollection.Instance.GetAll();
            TableFirePowerCollection.Instance.GetAll();
            yield return null;
            TableFireSpeedCollection.Instance.GetAll();
            TableGameLevelCollection.Instance.GetAll();
            yield return null;
            TableGameWaveCollection.Instance.GetAll();
            TableLanguageCollection.Instance.GetAll();
            yield return null;
            TableVirusCollection.Instance.GetAll();
            TableWeaponCollection.Instance.GetAll();
            yield return null;
            TableWeaponPowerLevelCollection.Instance.GetAll();
            TableWeaponSpeedLevelCollection.Instance.GetAll();
            TableShopCollection.Instance.GetAll();

            yield return null;

            TaskFinish();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (taskFinished)
                StateManager.ChangeState<MainState>();
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Close<LoadingView>();
        }
    }
}