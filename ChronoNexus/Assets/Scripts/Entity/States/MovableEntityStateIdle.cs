using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MovableEntityStateIdle : MovableEntityState
{

    public MovableEntityStateIdle(MovableEntity movableEntity, StateMachine stateMachine):base(movableEntity, stateMachine)
    {
    }

    public override void Enter()
    {
        _movableEntity.EndMoveAnimation();
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}