using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class LoadingState : StateBase
    {
        private bool taskFinished;

        public float progress { get { return 0; } private set { SplashView.SetProgress(value); } }
        public string message { get { return ""; } private set { SplashView.SetMessage(value); } }

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

        bool isAssetsInited = false;
        IEnumerator TaskAll()
        {
            mTaskWait.Clear();
            yield return null;

            // #1
            progress = 0.1f;
            message = LTKey.LOADING_INITIALIZE_ANALYTICS.LT();
            yield return null;
            ProxyManager.Subscribe<AnalyticsProxy>();

            // #2 Remote Config
            progress = 0.2f;
            message = LTKey.LOADING_INITIALIZE_REMOTE_CONFIG.LT();
            yield return null;
            ProxyManager.Subscribe<RemoteConfigProxy>();

            // #3
            progress = 0.3f;
            message = LTKey.LOADING_INITIALIZE_DATA.LT();
            yield return null;
            ProxyManager.Subscribe<DataProxy>();

            // #4
            progress = 0.4f;
            message = LTKey.LOADING_INITIALIZE_ADVERTISEMENT.LT();
            yield return null;
            ProxyManager.Subscribe<AdProxy>();

            // #5
            progress = 0.5f;
            message = LTKey.LOADING_INITIALIZE_IN_APP_PURCHASE.LT();
            yield return null;
            // IAPManager.Instance.Init();

            // #6
            progress = 0.6f;
            message = LTKey.LOADING_INITIALIZE_SETTINGS.LT();
            yield return null;
            Application.targetFrameRate = CT.table.frameRate;

            // #7
            progress = 0.7f;
            message = LTKey.LOADING_INITIALIZE_RESOURCES_ATLAS.LT();
            yield return null;
            UIUtil.LoadAtlasAll();

            // #8
            progress = 0.8f;
            message = LTKey.LOADING_INITIALIZE_RESOURCES_ENTITIES.LT();
            yield return null;
            EntityManager.WarmPoolAll();

            // #9
            progress = 0.9f;
            message = LTKey.LOADING_INITIALIZE_RESOURCES_VIEWS.LT();
            yield return null;
            UIManager.Load<BattleView>();

            // #10
            progress = 1f;
            message = LTKey.LOADING_INITIALIZE_RESOURCES_SOUNDS.LT();
            AudioManager.Preload("BGM1");
            AudioManager.Preload("BGM2");
            AudioManager.Preload("BGM3");
            AudioManager.Preload("BGM4");
            AudioManager.Preload("hit");

            // #11
            progress = 1f;
            message = LTKey.LOADING_INITIALIZE_RESOURCES_TABLES.LT();
            TableAdsCollection.Instance.GetAll();
            TableAircraftCollection.Instance.GetAll();
            TableBuffCollection.Instance.GetAll();
            TableBuffAutoGenCollection.Instance.GetAll();
            TableBuffKillGenCollection.Instance.GetAll();
            TableCoinIncomeCollection.Instance.GetAll();
            TableCoinValueCollection.Instance.GetAll();
            TableConstCollection.Instance.GetAll();
            TableDailySignCollection.Instance.GetAll();
            TableFirePowerCollection.Instance.GetAll();
            TableFireSpeedCollection.Instance.GetAll();
            TableGameLevelCollection.Instance.GetAll();
            TableGameWaveCollection.Instance.GetAll();
            TableLanguageCollection.Instance.GetAll();
            TableVirusCollection.Instance.GetAll();
            TableWeaponCollection.Instance.GetAll();
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
            {
                StateManager.ChangeState<MainState>();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
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
    }
}