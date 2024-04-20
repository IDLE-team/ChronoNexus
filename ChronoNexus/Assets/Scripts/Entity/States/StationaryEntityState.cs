using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class StationaryEntityState : IState
{
    protected StationaryEntity _stationaryEntity;
    protected StateMachine _stateMachine;

    protected StationaryEntityState(StationaryEntity stationaryEntity, StateMachine stateMachine)
    {
        _stationaryEntity = stationaryEntity;
        _stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        TimeWaiter().Forget();
    }

    public virtual void LogicUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Exit()
    {
        
    }
    protected virtual async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_stationaryEntity.isTimeSlowed && !_stationaryEntity.isTimeStopped);
        
    }
}