using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace DestroyViruses
{
    public class LoadingState : State
    {
        float m_waitSeconds = 0;
        public override void OnEnter()
        {
            m_waitSeconds = 3;
            base.OnEnter();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            m_waitSeconds -= deltaTime;
            if (m_waitSeconds <= 0)
                GameManager.Instance.stateMachine.currentState = new MainState();
        }
    }
}