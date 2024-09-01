using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public abstract class MovableEntityState : IState
{
    protected MovableEntity _movableEntity;
    protected StateMachine _stateMachine;
    protected NavMeshAgent _navMeshAgent;
    protected MovableEntityState(MovableEntity movableEntity, StateMachine stateMachine)
    {
        _movableEntity = movableEntity;
        _stateMachine = stateMachine;
        _navMeshAgent = movableEntity.NavMeshAgent;
    }
    public virtual void Enter()
    {
        TimeWaiter().Forget();
    }
    public virtual void Exit()
    {
    }
    public virtual void LogicUpdate()
    {
    }
    public virtual void PhysicsUpdate()
    {
    } 
    protected virtual async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableEntity.isTimeSlowed && !_movableEntity.isTimeStopped);
    }
}