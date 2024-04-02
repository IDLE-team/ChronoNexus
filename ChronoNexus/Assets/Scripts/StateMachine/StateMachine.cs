using System;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }
    public event Action OnStateChanged;
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
        OnStateChanged?.Invoke();

    }

    public void ChangeState(IState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        newState.Enter();
        OnStateChanged?.Invoke();
    }
}