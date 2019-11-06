using UnityEngine;
using System.Reflection;
using System;

public class StateManager : Singleton<StateManager>
{
    public string startupState;
    public StateMachine.State currentState => mStateMachine.currentState;

    private StateMachine.StateMachine<StateMachine.State> mStateMachine;

    void Awake()
    {
        mStateMachine = new StateMachine.StateMachine<StateMachine.State>();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(startupState))
        {
            Type type = Assembly.Load("Assembly-CSharp").GetType(startupState);
            mStateMachine.currentState = (StateMachine.State)Activator.CreateInstance(type);
        }
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
