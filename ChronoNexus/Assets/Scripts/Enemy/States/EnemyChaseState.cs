using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Vector3 _playerPosition;

    public EnemyChaseState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
        _enemy.canSeeTarget = false;
    }

    public override void LogicUpdate()
    {
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
        _playerPosition = _enemy.player.position;
        _enemy.NavMeshAgent.SetDestination(_playerPosition);
    }
}