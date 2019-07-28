using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        void Update()
        {
            stateMachine.OnUpdate(Time.deltaTime);
        }
    }
}