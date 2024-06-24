using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class MovableEntityStateChase : MovableEntityState
{
    public MovableEntityStateChase(MovableEntity movableEntity, StateMachine stateMachine) : base(movableEntity,
        stateMachine)
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
        _movableEntity.TargetChaseDistanceSwitch();
        base.LogicUpdate();
    }
    public override void PhysicsUpdate()
    {
        _movableEntity.AgentDestinationSet();
        base.PhysicsUpdate();
    }
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableEntity.isTimeSlowed && !_movableEntity.isTimeStopped);
    }
}