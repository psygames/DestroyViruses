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
            UIManager.Open<MainView>();
            UIManager.Open<NavigationView>(UILayer.Top);
        }

        public override void OnExit()
        {
            base.OnExit();
            Aircraft.ins.anima.KillAll();
            Aircraft.ins.weapon.Reset();
            UIManager.Close<MainView>();
            UIManager.Close<NavigationView>();
        }
    }
}