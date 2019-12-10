using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class LoadingState : StateBase
    {
        float m_waitSeconds = 0;
        public override void OnEnter()
        {
            m_waitSeconds = 0.1f;
            UIUtil.LoadAtlasAll();
            ProxyManager.Subscribe<AdProxy>();
            Analytics.Event.Login(DeviceID.UUID);
            GDM.ins.AnalyticsSetUserProperty();
            Application.targetFrameRate = ConstTable.table.frameRate;
            base.OnEnter();
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