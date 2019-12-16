using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class MainState : StateBase
    {
        Aircraft aircraft = null;
        public override void OnEnter()
        {
            base.OnEnter();
            aircraft = Aircraft.Create();
            aircraft.Reset();
            aircraft.anima.PlayStandby();
            UIManager.Instance.Open<MainView>();
            UIManager.Instance.Open<NavigationView>(UILayer.Top);
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
            aircraft.anima.StopAll();
            UIManager.Instance.Close<MainView>();
            UIManager.Instance.Close<NavigationView>();
            GDM.ins.SaveLocalData();
        }
    }
}