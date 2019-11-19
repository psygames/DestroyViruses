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
            UIManager.Instance.Open<BattleView>();
            GameModeManager.Instance.InitMode<LevelMode>();
            GameModeManager.Instance.Begin();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Instance.Close<BattleView>();
            EntityManager.Clear();
            GDM.ins.SaveLocalData();
            GDM.ins.gameEndWin = false;
        }
    }
}