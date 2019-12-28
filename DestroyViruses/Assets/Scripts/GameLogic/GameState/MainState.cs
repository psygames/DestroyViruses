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
            Aircraft.ins.anima.PlayHomeIn();
            UIManager.Instance.Open<MainView>();
            UIManager.Instance.Open<NavigationView>(UILayer.Top);
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
            Aircraft.ins.anima.KillAll();
            UIManager.Instance.Close<MainView>();
            UIManager.Instance.Close<NavigationView>();
        }
    }
}