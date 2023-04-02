using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyRangeAttackState : EnemyState
{
    //ToDo Улучшить систему

    private Transform _target;
    private Vector3 _playerPosition;
    private float shootingTimer = 0;
    private float shootingInterval = 1f;
    private float retreatDistance = 5f;
    private float minDelay = 3f;
    private float maxDelay = 4f;
    private bool _isAttack = false;

    public EnemyRangeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        _target = _enemy.player.transform;
        _enemy.NavMeshAgent.speed = 2.5f;
        _isAttack = true;
        ShootAndRetreat().Forget();
    }

    public override void Exit()
    {
        _isAttack = false;
        _enemy.canSeeTarget = false;
        _enemy.NavMeshAgent.speed = 1.5f;
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
        _target = _enemy.player.transform;
    }

    private async UniTask ShootAndRetreat()
    {
        while (_isAttack)
        {
            await UniTask.Delay((int)(Random.Range(minDelay, maxDelay) * 1000));

            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            Vector3 retreatPosition = _target.position + randomDirection * retreatDistance;
            retreatPosition = new Vector3(retreatPosition.x, _enemy.transform.position.y, retreatPosition.z);

            float distanceToTarget = Vector3.Distance(retreatPosition, _target.position);
            Debug.Log(distanceToTarget);
            if (distanceToTarget < 5f)
            {
                retreatPosition += randomDirection * (5f - distanceToTarget);
            }
            if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.1f)
            {
                _enemy.NavMeshAgent.SetDestination(retreatPosition);
                await UniTask.Yield();
            }
        }
    }
}