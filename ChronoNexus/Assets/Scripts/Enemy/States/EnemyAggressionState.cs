using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class EnemyAggressionState : EnemyState
{
    private Transform _target;
    private Vector3 _targetPosition;
    private float attackTimer = 0;
    private float attackInterval = 1f;
    private float retreatDistance = 2.5f;
    private float minDelay = 0.1f;
    private float maxDelay = 0.5f;
    private bool _isAttack = false;
    //private int damageMultiplayer = 2;

    //Для ближников: Бежит на игрока с оружием ближнего боя, скорость атаки увеличена 
    //Для дальников: Экипирует стандартное оружие ближнего боя

    private CancellationTokenSource cancellationTokenSource;

    public EnemyAggressionState(Enemy enemy, StateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        switch (_enemy.enemyType)
        {
            case Enemy.EnemyType.Stormtrooper:
                
                break;
            case Enemy.EnemyType.Guard:
                _enemy.EnemyAttacker.MultiplyAttackInterval(0.5f);
                break;
            default:
                break;
        }

        //_enemy.GetComponent<EnemyAttacker>().MultiplyDamage(damageMultiplayer);

        cancellationTokenSource = new CancellationTokenSource();
        _target = _enemy.Target.transform;
        _enemy.NavMeshAgent.speed = 2.5f;
        _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
        _isAttack = true;
        _enemy.EndMoveAnimation();
        MeleeAttackAndRetreat(cancellationTokenSource.Token).Forget();
    }

    public override void Exit()
    {
        _isAttack = false;
        _enemy.IsTargetFound = false;
        _enemy.NavMeshAgent.speed = 1.5f;
        //_enemy.EndMoveAnimation();
    }

    public override void LogicUpdate()
    {
        if (_enemy.Target == null)
        {
            cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_enemy.PatrolState);
            return;
        }
        
        _targetPosition = _enemy.Target.position;
        _target = _enemy.Target.transform;

        Vector3 targetPosition = new Vector3(
            _targetPosition.x,
            _enemy.transform.position.y,
            _targetPosition.z
        );

        _enemy.transform.LookAt(targetPosition);

        if (Vector3.Distance(_enemy.transform.position, _targetPosition) > 2f)
        {
            _enemy.NavMeshAgent.SetDestination(_target.position);
            //_stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0 && Vector3.Distance(_enemy.transform.position, _targetPosition) <= 2f)
        {
            _enemy.StartAttackAnimation();
            _enemy.MeleeAttack();
            attackTimer = attackInterval;
        }
    }

    public override void PhysicsUpdate()
    {

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
                Vector3 randomDirection = Random.onUnitSphere.normalized/2;
                Vector3 retreatPosition = _target.position + randomDirection * retreatDistance;

                retreatPosition = new Vector3(
                    retreatPosition.x,
                    _enemy.transform.position.y,
                    retreatPosition.z
                );

                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.StartMoveAnimation();

                    await UniTask.Yield();
                }
                else
                {
                    _enemy.EndMoveAnimation();
                }
            }
        }
        await UniTask.Yield();
    }
}
