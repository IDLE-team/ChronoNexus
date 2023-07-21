using System.Threading;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Vector3 _playerPosition;

    public EnemyChaseState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        _enemy.StartMoveAnimation();
    }

    public override void Exit()
    {
        _enemy.IsTargetFound = false;
    }

    public override void LogicUpdate()
    {
        if (_enemy.Target == null)
        {
            _stateMachine.ChangeState(_enemy.DummyState);
            return;
        }
        if (Vector3.Distance(_enemy.transform.position, _playerPosition) > 10f)
        {
            _stateMachine.ChangeState(_enemy.PatrolState);
        }
        if (Vector3.Distance(_enemy.transform.position, _playerPosition) < 8f)
        {
            _stateMachine.ChangeState(_enemy.RangeAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (_enemy.Target == null)
        {
            _stateMachine.ChangeState(_enemy.DummyState);
            return;
        }
        _playerPosition = _enemy.Target.position;
        _enemy.NavMeshAgent.SetDestination(_playerPosition);
    }
}