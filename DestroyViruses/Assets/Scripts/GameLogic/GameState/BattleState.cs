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
            Aircraft.Create();
        }

        public override void OnExit()
        {
            base.OnExit();
            EntityManager.Clear();
        }
    }
}