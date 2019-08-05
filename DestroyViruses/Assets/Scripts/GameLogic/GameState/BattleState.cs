using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class BattleState : StateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            UIManager.Instance.Open<BattlePanel>();
            GameModeManager.Instance.StartMode<LevelMode>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Instance.Close<BattlePanel>();
            EntityManager.Clear();
        }
    }
}