using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class BattleState : StateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            UIManager.Instance.Open<BattlePanel>();
            Aircraft.Create();
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Instance.Close<BattlePanel>();
            EntityManager.Clear();
        }
    }
}