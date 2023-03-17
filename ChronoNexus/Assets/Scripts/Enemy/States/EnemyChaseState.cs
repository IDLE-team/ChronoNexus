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
        _enemy.canSeePlayer = false;
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(_enemy.transform.position, _playerPosition) > 10f)
        {
            _enemy.StateMachine.ChangeState(_enemy.PatrolState);
        }
    }

    public override void PhysicsUpdate()
    {
        _playerPosition = _enemy.player.position;
        _enemy.navMeshAgent.SetDestination(_playerPosition);
    }
}