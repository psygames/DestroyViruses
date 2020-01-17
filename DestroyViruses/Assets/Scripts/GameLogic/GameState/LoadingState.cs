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

        IEnumerator TaskAll()
        {
            // #0 Delay One Frame
            yield return null;

            // #1
            progress += step;
            message = "Initialize Data...";
            ProxyManager.Subscribe<DataProxy>();
            yield return null;

            // #2
            progress += step;
            message = "Initialize Analytics...";
            ProxyManager.Subscribe<AnalyticsProxy>();
            yield return null;
            while (!AnalyticsProxy.Ins.isInit)
                yield return null;

            // #3
            progress += step;
            message = "Initialize Advertisement...";
            ProxyManager.Subscribe<AdProxy>();
            yield return null;
            while (!AdProxy.Ins.isInit)
                yield return null;

            // #4
            progress += step;
            message = "Initialize Settings...";
            Application.targetFrameRate = ConstTable.table.frameRate;
            yield return null;

            // #5
            progress += step;

            message = "Preload Resources(Atlas)...";
            UIUtil.LoadAtlasAll();
            yield return null;

            message = "Preload Resources(Entities)...";
            EntityManager.WarmPoolAll();
            yield return null;

            message = "Preload Resources(Views)...";
            UIManager.Load<BattleView>();
            yield return null;

            message = "Preload Resources(Sounds)...";
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

            message = "Preload Resources(Tables)...";
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