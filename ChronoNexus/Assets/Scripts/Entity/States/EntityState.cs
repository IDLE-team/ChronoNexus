using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class EntityState : IState
{
    protected Entity _entity;
    protected StateMachine _stateMachine;

    protected EntityState(Entity entity, StateMachine stateMachine)
    {
        _entity = entity;
        _stateMachine = stateMachine;
    }
    public abstract void Enter();

    public abstract void Exit();

    public abstract void LogicUpdate();

    public abstract void PhysicsUpdate();
    
}