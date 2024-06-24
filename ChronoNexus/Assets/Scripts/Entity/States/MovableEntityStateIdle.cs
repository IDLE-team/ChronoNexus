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
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableEntity.isTimeSlowed && !_movableEntity.isTimeStopped);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}