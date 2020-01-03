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
            ProxyManager.Subscribe<BuffProxy>();
            UIManager.Open<BattleView>();
            GameModeManager.Instance.InitMode<LevelMode>();
            GameModeManager.Instance.Begin();
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Close<BattleView>();
            EntityManager.Clear();
            D.I.gameEndWin = false;
            ProxyManager.Unsubscribe<BuffProxy>();
        }
    }
}