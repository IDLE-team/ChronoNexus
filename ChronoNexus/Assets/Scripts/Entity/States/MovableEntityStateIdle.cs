using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MovableEntityStateIdle : MovableEntityState
{

    public MovableEntityStateIdle(MovableEntity movableEntity, StateMachine stateMachine):base(movableEntity, stateMachine)
    {
    }

    public override void Enter()
    {
        //_movableEntity.EntityAnimator.SetMoveAnimation(false);
        _movableEntity.StartSeek();
        base.Enter();
    }

    public override void Exit()
    {
        _movableEntity.StopSeek();
        base.Exit();
    }

    public override void LogicUpdate()
    {
        CheckTarget();
        
        base.LogicUpdate();
    }
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableEntity.isTimeSlowed && !_movableEntity.isTimeStopped);
        //_movableEntity.NavMeshAgent.speed = _movableEntity.ChaseSpeed;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}