using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class SplashState : StateBase
    {
        float m_min = 0;
        float m_max = 0;
        public override void OnEnter()
        {
            base.OnEnter();
            m_min = 0.5f;
            m_max = 1f;

            ProxyManager.Subscribe<AnalyticsProxy>();
            ProxyManager.Subscribe<DataProxy>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            m_min -= deltaTime;
            m_max -= deltaTime;
            if (CanPass())
            {
                StateManager.ChangeState<HotUpdateState>();
            }
        }

        private bool CanPass()
        {
            if (m_max <= 0)
                return true;
            if (m_min <= 0)
            {
                if (ProxyManager.GetProxy<AnalyticsProxy>().isInit)
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnExit()
        {
            Analytics.Event.AppOpen();
            base.OnExit();
        }
    }
}