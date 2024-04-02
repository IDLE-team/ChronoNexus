using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyIdleState : EnemyHumanoidState
{
    public EnemyIdleState(EnemyHumanoid enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        _enemy.EndMoveAnimation();
        _enemy.StartSeek();
    }

    public override void LogicUpdate()
    {
        if (_enemy.IsTargetFound)
        {

            StateReactionToTarget();
            return;
        }
    }

    protected virtual void StateReactionToTarget()
    {
        _stateMachine.ChangeState(_enemy.ChaseState);
    }

    public override void Exit()
    {
        _enemy.StopSeek();
    }

    public override void PhysicsUpdate()
    {
        
    }
}