using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class GameManager : Singleton<GameManager>
    {
        public StateMachine.StateMachine<StateMachine.State> stateMachine { get; private set; }

        void Awake()
        {
            stateMachine = new StateMachine.StateMachine<StateMachine.State>();
        }

        void Start()
        {
            stateMachine.currentState = new LoadingState();
            Observable.Interval(TimeSpan.FromSeconds(3)).Do((ticks) =>
            {
                var virus = VirusBase.Create();
                virus.Reset(new Vector2(UIUtil.width * 0.5f, UIUtil.height));
            }).Subscribe();
        }

        void Update()
        {
            stateMachine.OnUpdate(Time.deltaTime);
        }
    }
}