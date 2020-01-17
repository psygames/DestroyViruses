using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class SplashState : StateBase
    {
        float m_max;
        public override void OnEnter()
        {
            base.OnEnter();
            m_max = 0.1f;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            m_max -= deltaTime;
            if (m_max <= 0)
            {
                StateManager.ChangeState<HotUpdateState>();
            }
        }
    }
}