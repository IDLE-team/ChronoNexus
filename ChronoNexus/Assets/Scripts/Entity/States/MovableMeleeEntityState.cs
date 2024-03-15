using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public abstract class MovableMeleeEntityState : IState
{
    protected MovableMeleeEntity _movableMeleeEntity;
    protected StateMachine _stateMachine;
    protected NavMeshAgent _navMeshAgent;

    protected MovableMeleeEntityState(MovableMeleeEntity movableMeleeEntity, StateMachine stateMachine)
    {
        _movableMeleeEntity = movableMeleeEntity;
        _stateMachine = stateMachine;
        _navMeshAgent = movableMeleeEntity.NavMeshAgent;
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
        await UniTask.WaitUntil(() => !_movableMeleeEntity.isTimeSlowed && !_movableMeleeEntity.isTimeStopped);
        _movableMeleeEntity.NavMeshAgent.speed = 1.5f;
        //Default speed
    }
}