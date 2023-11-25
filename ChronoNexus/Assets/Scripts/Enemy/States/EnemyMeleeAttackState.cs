using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyMeleeAttackState : EnemyState
{
    private Transform _target;
    private Vector3 _targetPosition;
    private float attackTimer = 0;
    private float attackInterval = 1f;
    private float retreatDistance = 5f;
    private float minDelay = 0.5f;
    private float maxDelay = 2f;
    private bool _isAttack = false;

    private CancellationTokenSource cancellationTokenSource;

    public EnemyMeleeAttackState(Enemy enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        cancellationTokenSource = new CancellationTokenSource();
        _target = _enemy.Target.transform;
        _enemy.NavMeshAgent.speed = 2.5f;
        _isAttack = true;
        MeleeAttackAndRetreat(cancellationTokenSource.Token).Forget();
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

        Vector3 targetPosition = new Vector3(
            _targetPosition.x,
            _enemy.transform.position.y,
            _targetPosition.z
        );
        _enemy.transform.LookAt(targetPosition);

        if (Vector3.Distance(_enemy.transform.position, _targetPosition) > 2f)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        attackTimer -= Time.deltaTime;

        // if (attackTimer <= 0f )
        // {
        //     attackTimer = attackInterval;
        // }
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

        // if (Vector3.Distance(_enemy.transform.position, _targetPosition) > 2f)
        // {
        //     _enemy.NavMeshAgent.SetDestination(_target.position);
        // }
    }

    private async UniTask MeleeAttackAndRetreat(CancellationToken cancellationToken)
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
                if (
                    attackTimer <= 0
                    && Vector3.Distance(_enemy.transform.position, _targetPosition) <= 2f
                )
                {
                    _enemy.StartAttackAnimation();
                    _enemy.NavMeshAgent.SetDestination(_target.position);
                    _enemy.MeleeAttack(); //spawn enemy blade
                    attackTimer = attackInterval;
                }
                else if (attackTimer > 0)
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                }

                await UniTask.Yield();
            }
        }
        await UniTask.Yield();
    }
}
