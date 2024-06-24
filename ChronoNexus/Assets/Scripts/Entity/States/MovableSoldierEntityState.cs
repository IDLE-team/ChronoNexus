using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public abstract class MovableSoldierEntityState : IState
{
    protected MovableSoldierEntity _movableSoldierEntity;
    protected StateMachine _stateMachine;
    protected NavMeshAgent _navMeshAgent;
    protected MovableSoldierEntityState(MovableSoldierEntity movableSoldierEntity, StateMachine stateMachine)
    {
        _movableSoldierEntity = movableSoldierEntity;
        _stateMachine = stateMachine;
        _navMeshAgent = movableSoldierEntity.NavMeshAgent;
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
        await UniTask.WaitUntil(() => !_movableSoldierEntity.isTimeSlowed && !_movableSoldierEntity.isTimeStopped);
    }
}