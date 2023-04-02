using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void LogicUpdate()
    {
        if (_enemy.canSeeTarget)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }
    }

    public override void Exit()
    {
    }

    public override void PhysicsUpdate()
    {
    }
}