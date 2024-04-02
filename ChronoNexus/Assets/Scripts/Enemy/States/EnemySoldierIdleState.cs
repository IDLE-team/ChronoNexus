using UnityEngine;

public class EnemySoldierIdleState : EnemySoldierState
{
    //private new EnemySoldier _enemy;
    public EnemySoldierIdleState(EnemySoldier enemy, StateMachine stateMachine) : base(enemy, stateMachine)
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
            _stateMachine.ChangeState(_enemy.ChaseState);
            
            return;
        }
    }

    public override void Exit()
    {
        _enemy.StopSeek();
    }

    public override void PhysicsUpdate()
    {
        
    }
}