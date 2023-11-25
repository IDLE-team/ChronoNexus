using System.Threading;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Vector3 _playerPosition;

    public EnemyChaseState(Enemy enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

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
        switch (_enemy.enemyType) // attack state select
        {
            case Enemy.EnemyType.Guard:
                if (Vector3.Distance(_enemy.transform.position, _playerPosition) <= 2f)
                {
                    _stateMachine.ChangeState(_enemy.MeleeAttackState);
                }
                break;
            case Enemy.EnemyType.Stormtrooper:
                if (Vector3.Distance(_enemy.transform.position, _playerPosition) < 8f)
                {
                    _stateMachine.ChangeState(_enemy.RangeAttackState);
                }
                break;
            default:
                if (Vector3.Distance(_enemy.transform.position, _playerPosition) < 8f)
                {
                    _stateMachine.ChangeState(_enemy.RangeAttackState);
                }
                break;
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
