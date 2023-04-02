using UnityEngine;

public class EnemyRangeAttackState : EnemyState
{
    private Vector3 _playerPosition;
    private float shootingTimer = 0;
    public float shootingInterval = 1f;

    public EnemyRangeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        _enemy.NavMeshAgent.stoppingDistance = 5f;
    }

    public override void Exit()
    {
        _enemy.NavMeshAgent.stoppingDistance = 1f;
        _enemy.canSeeTarget = false;
    }

    public override void LogicUpdate()
    {
        Vector3 targetPosition = new Vector3(_playerPosition.x, _enemy.transform.position.y, _playerPosition.z);
        _enemy.transform.LookAt(targetPosition);

        if (Vector3.Distance(_enemy.transform.position, _playerPosition) > 8f)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0f)
        {
            _enemy.Shoot(_playerPosition);
            shootingTimer = shootingInterval;
        }
    }

    public override void PhysicsUpdate()
    {
        _playerPosition = _enemy.player.position;

        if (Vector3.Distance(_enemy.transform.position, _playerPosition) > 5)
        {
            _enemy.NavMeshAgent.SetDestination(_playerPosition);
        }
        else return;
    }
}