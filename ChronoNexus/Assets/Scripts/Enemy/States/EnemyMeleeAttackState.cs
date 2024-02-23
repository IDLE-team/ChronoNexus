using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EnemyMeleeAttackState : EnemyState
{
    private Transform _target;
    private Vector3 _targetPosition;

    private float attackTimer;
    private float attackInterval ;

    private float minDelay = 1f;
    private float maxDelay = 2f;

    private float _maxDistanceBetweenTarget = 1.5f;
    private float _minDistanceBetweenTarget = 0.8f;

    private bool _isAttack = false;

    private float _defaultAgentSpeed = 1.5f;
    private float _attackingAgentSpeed = 2.5f;

    Vector3 randomDirection;
    Vector3 retreatPosition;
    //Vector3 targetPosition;
    Quaternion toRotation;




    private CancellationTokenSource cancellationTokenSource;

    public EnemyMeleeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        attackInterval = _enemy.EnemyAttacker.MeleeAttackInterval;

        _target = _enemy.Target.transform;
        _targetPosition = _target.position;

        _isAttack = true;

        //_enemy.NavMeshAgent.speed = 2.5f;

        //_enemy.NavMeshAgent.SetDestination(_enemy.transform.position);

        //_enemy.EndMoveAnimation();

        cancellationTokenSource = new CancellationTokenSource();
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

        

        //targetPosition = _targetPosition - _enemy.transform.position;
        toRotation = Quaternion.LookRotation(_targetPosition - _enemy.transform.position, Vector3.up);

        if (_enemy.isTimeSlowed)
        {
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
        }
        else
        {
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * Time.deltaTime);
        }

        if (Vector3.Distance(_enemy.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        if (attackTimer > 0)
        {
            if (_enemy.isTimeSlowed)
            {
                attackTimer -= (Time.deltaTime * 0.2f);
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }

        if (attackTimer <= 0)
        {
            _enemy.StartAttackAnimation();
            //_enemy.EndMoveAnimation();
            //_enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
            _enemy.MeleeAttack();
            attackTimer = attackInterval;
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

    private async UniTask MeleeAttackAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int)(Random.Range(minDelay, maxDelay) * 1000));

            if (_enemy == null)
                cancellationTokenSource.Cancel();
            if (_enemy.Target == null)
                cancellationTokenSource.Cancel();
            if (Vector3.Distance(_enemy.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();


            if (!cancellationToken.IsCancellationRequested
                //&& !_isReloading
                && Vector3.Distance(_enemy.transform.position, _targetPosition) <= _maxDistanceBetweenTarget
                && Vector3.Distance(_enemy.transform.position, _targetPosition) > _minDistanceBetweenTarget
                )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _enemy.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget)/2);
                    retreatPosition = new Vector3(retreatPosition.x, _enemy.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) < _minDistanceBetweenTarget || Vector3.Distance(_targetPosition, retreatPosition) > _maxDistanceBetweenTarget);


                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.2f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.NavMeshAgent.speed = _attackingAgentSpeed;
                    _enemy.StartMoveAnimation();
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                    _enemy.EndMoveAnimation();
                }
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested
                 //&& !_isReloading
                 && Vector3.Distance(_enemy.transform.position, _targetPosition) <= _minDistanceBetweenTarget
                 )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _enemy.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _enemy.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) <= _minDistanceBetweenTarget);


                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.2f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.NavMeshAgent.speed = _attackingAgentSpeed;
                    _enemy.StartMoveAnimation();
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                    _enemy.EndMoveAnimation();
                }
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested
                //&& _isReloading
                )
            {
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                _enemy.NavMeshAgent.speed = _defaultAgentSpeed;
                _enemy.EndMoveAnimation();
                await UniTask.Yield();
            }
            /*await UniTask.Delay((int)(Random.Range(minDelay, maxDelay) * 1000));

            if (_enemy == null)
                cancellationTokenSource.Cancel();
            if (_enemy.Target == null)
                cancellationTokenSource.Cancel();


            if (!cancellationToken.IsCancellationRequested)
            {
                Vector3 randomDirection = Random.onUnitSphere.normalized;
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
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                    _enemy.EndMoveAnimation();
                }
                await UniTask.Yield();
            }*/
        }
        await UniTask.Yield();
    }
}
