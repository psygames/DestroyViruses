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
            Aircraft.Create().Reset();
            UIManager.Instance.Open<MainPanel>();
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
            UIManager.Instance.Close<MainPanel>();
            GDM.ins.SaveLocalData();
        }
    }
}