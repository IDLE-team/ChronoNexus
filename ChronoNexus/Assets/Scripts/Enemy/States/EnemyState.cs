using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class EnemyState : IState
{
    protected Enemy _enemy;
    protected StateMachine _stateMachine;

    protected EnemyState(Enemy enemy, StateMachine stateMachine)
    {
        _enemy = enemy;
        _stateMachine = stateMachine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void LogicUpdate();

    public abstract void PhysicsUpdate();
}