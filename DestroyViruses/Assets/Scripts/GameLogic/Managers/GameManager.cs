using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace DestroyViruses
{
    public class GameManager : Singleton<GameManager>
    {
        private StateMachine.StateMachine<StateMachine.State> mStateMachine;

        void Awake()
        {
            mStateMachine = new StateMachine.StateMachine<StateMachine.State>();
        }

        void Start()
        {
            ChangeState<LoadingState>();
        }

        void Update()
        {
            mStateMachine.OnUpdate(Time.deltaTime);
        }


        public static void ChangeState<T>() where T : StateMachine.State, new()
        {
            Instance.mStateMachine.currentState = new T();
        }
    }
}