using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class MainState : StateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            UIManager.Instance.Open<MainPanel>();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            Change<BattleState>();
        }
    }
}