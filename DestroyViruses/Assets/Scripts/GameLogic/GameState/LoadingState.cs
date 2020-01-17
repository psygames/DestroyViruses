using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class LoadingState : StateBase
    {
        public float progress { get; private set; }
        public string message { get; private set; }

        float m_waitSeconds;
        public override void OnEnter()
        {
            base.OnEnter();
            m_waitSeconds = 0.3f;
            ProxyManager.Subscribe<AnalyticsProxy>();
            ProxyManager.Subscribe<DataProxy>();
            ProxyManager.Subscribe<AdProxy>();
            Analytics.Event.Login(DeviceID.UUID);
            D.I.AnalyticsSetUserProperty();
            Application.targetFrameRate = ConstTable.table.frameRate;
            PreloadAll();
        }

        private void PreloadAll()
        {
            UIUtil.LoadAtlasAll();
            EntityManager.WarmPoolAll();
            UIManager.Load<BattleView>();

            AudioManager.Preload("BGM1");
            AudioManager.Preload("BGM2");
            AudioManager.Preload("BGM3");
            AudioManager.Preload("BGM4");
            AudioManager.Preload("hit");

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
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            m_waitSeconds -= deltaTime;
            if (m_waitSeconds <= 0)
                StateManager.ChangeState<MainState>();
        }
    }
}