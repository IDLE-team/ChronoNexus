using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class EnemyState : IState
{
    protected Enemy _enemy;

    protected EnemyState(Enemy enemy, StateMachine stateMachine)
    {
        this._enemy = enemy;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void LogicUpdate();

    public abstract void PhysicsUpdate();
}