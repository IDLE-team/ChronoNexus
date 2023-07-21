using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyRangeAttackState : EnemyState
{
    //ToDo Улучшить систему

    private Transform _target;
    private Vector3 _targetPosition;
    private float shootingTimer = 0;
    private float shootingInterval = 2f;
    private float retreatDistance = 5f;
    private float minDelay = 3f;
    private float maxDelay = 4f;
    private bool _isAttack = false;

    private CancellationTokenSource cancellationTokenSource;

    public EnemyRangeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        cancellationTokenSource = new CancellationTokenSource();
        _target = _enemy.Target.transform;
        _enemy.NavMeshAgent.speed = 2.5f;
        _isAttack = true;
        ShootAndRetreat(cancellationTokenSource.Token).Forget();
    }

    public override void Exit()
    {
        _isAttack = false;
        _enemy.IsTargetFound = false;
        _enemy.NavMeshAgent.speed = 1.5f;
        _enemy.EndMoveAnimation();
    }

    public override void LogicUpdate()
    {
        if (_enemy.Target == null)
        {
            cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_enemy.PatrolState);
            return;
        }

        Vector3 targetPosition = new Vector3(_targetPosition.x, _enemy.transform.position.y, _targetPosition.z);
        _enemy.transform.LookAt(targetPosition);
        if (Vector3.Distance(_enemy.transform.position, _targetPosition) > 8f)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0f)
        {
            _enemy.Shoot(_targetPosition);
            shootingTimer = shootingInterval;
        }
    }

    public override void PhysicsUpdate()
    {
        if (_enemy.Target == null)
        {
            cancellationTokenSource.Cancel();

            return;
        }
        _targetPosition = _enemy.Target.position;
        _target = _enemy.Target.transform;
    }

    private async UniTask ShootAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int)(Random.Range(minDelay, maxDelay) * 1000));

            if (_enemy == null)
                cancellationTokenSource.Cancel();
            if (_enemy.Target == null)
                cancellationTokenSource.Cancel();

            if (!cancellationToken.IsCancellationRequested)
            {
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                Vector3 retreatPosition = _target.position + randomDirection * retreatDistance;
                retreatPosition = new Vector3(retreatPosition.x, _enemy.transform.position.y, retreatPosition.z);

                float distanceToTarget = Vector3.Distance(retreatPosition, _target.position);

                if (distanceToTarget < 5f)
                {
                    retreatPosition += randomDirection * (5f - distanceToTarget);
                }
                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.StartMoveAnimation();

                    await UniTask.Yield();
                }
            }
        }
        await UniTask.Yield();
    }
}