using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EnemyMeleeAttackState : EnemyHumanoidState
{
    public EnemyMeleeAttackState(EnemyHumanoid enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

    protected Vector3 _targetPosition;

    private float _attackTimer;
    private float _attackInterval = 1f;

    protected float _minDelay = 0.3f;
    protected float _maxDelay = 1f;

    protected float _maxDistanceBetweenTarget = 2f;
    protected float _minDistanceBetweenTarget = 1f;

    private bool _isAttack = false;

    private float _meleeAttackingAgentSpeed = 4f;

    protected Vector3 randomDirection;
    protected Vector3 retreatPosition;
    protected Quaternion toRotation;

    private CancellationTokenSource cancellationTokenSource;


    public override void Enter()
    {
        _attackInterval = _enemy.EnemyAttacker.MeleeAttackInterval;
        
        _minDelay = _enemy.EnemyAttacker.MinDelayTokenMelee;
        _maxDelay = _enemy.EnemyAttacker.MaxDelayTokenMelee;
        _maxDistanceBetweenTarget = _enemy.EnemyAttacker.MaxMeleeAttackDistance;
        _minDistanceBetweenTarget = _enemy.EnemyAttacker.MinMeleeDistanceToTarget;
        _meleeAttackingAgentSpeed = _enemy.EnemyAttacker.MeleeAttackAgentSpeed;

        _targetPosition = _enemy.Target.position;
        _isAttack = true;

        cancellationTokenSource = new CancellationTokenSource();
        MeleeAttackAndRetreat(cancellationTokenSource.Token).Forget();
    }

    public override void Exit()
    {
        _isAttack = false;
        _enemy.IsTargetFound = false;
        _enemy.NavMeshAgent.speed = 1.5f;
        _enemy.TargetFinder.SetWeight(0);
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

        

        toRotation = Quaternion.LookRotation(_targetPosition - _enemy.SelfAim.transform.position, Vector3.up);

        if (_enemy.isTimeSlowed)
        {
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
        }
        else
        {
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * Time.deltaTime);
        }

        if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        if (_attackTimer > 0)
        {
            if (_enemy.isTimeSlowed)
            {
                _attackTimer -= (Time.deltaTime * 0.2f);
            }
            else
            {
                _attackTimer -= Time.deltaTime;
            }
        }

        if (_attackTimer <= 0)
        {
            _enemy.StartAttackAnimation();
            //_enemy.EndMoveAnimation();
            //_enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
            _enemy.MeleeAttack();
            _attackTimer = _attackInterval;
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
        
        if (Vector3.Distance(_enemy.SelfAim.transform.position, retreatPosition) > 0.3f)
        {
            _enemy.StartMoveAnimation();
        }
        else
        {
            _enemy.EndMoveAnimation();
        }

    }

    private async UniTask MeleeAttackAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int)(Random.Range(_minDelay, _maxDelay) * 1000));

            if (_enemy == null)
                cancellationTokenSource.Cancel();
            if (_enemy.Target == null)
                cancellationTokenSource.Cancel();
            if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();


            if (!cancellationToken.IsCancellationRequested
                //&& !_isReloading
                && Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosition) <= _maxDistanceBetweenTarget
                && Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosition) > _minDistanceBetweenTarget
                )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _enemy.SelfAim.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget)/2);
                    retreatPosition = new Vector3(retreatPosition.x, _enemy.SelfAim.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) < _minDistanceBetweenTarget || Vector3.Distance(_targetPosition, retreatPosition) > _maxDistanceBetweenTarget);


                if (Vector3.Distance(_enemy.SelfAim.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.NavMeshAgent.speed = _meleeAttackingAgentSpeed;
                    _enemy.StartMoveAnimation();
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.SelfAim.transform.position);
                    _enemy.EndMoveAnimation();
                }
                _enemy.TargetFinder.SetWeight(1);
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested
                 //&& !_isReloading
                 && Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosition) <= _minDistanceBetweenTarget
                 )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _enemy.SelfAim.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _enemy.SelfAim.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) <= _minDistanceBetweenTarget);


                if (Vector3.Distance(_enemy.SelfAim.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    _enemy.NavMeshAgent.speed = _meleeAttackingAgentSpeed;
                    _enemy.StartMoveAnimation();
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.SelfAim.transform.position);
                    _enemy.EndMoveAnimation();
                }
                _enemy.TargetFinder.SetWeight(1);
                await UniTask.Yield();
            }
            /*else if (!cancellationToken.IsCancellationRequested
                && _isReloading
                )
            {
                _enemy.NavMeshAgent.SetDestination(_enemy.SelfAim.transform.position);
                _enemy.NavMeshAgent.speed = _defaultAgentSpeed;
                _enemy.EndMoveAnimation();
                await UniTask.Yield();
            }*/
        }
        await UniTask.Yield();
    }
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_enemy.isTimeSlowed && !_enemy.isTimeStopped);
        //_enemy.NavMeshAgent.speed *= 2;
        _enemy.NavMeshAgent.speed = _meleeAttackingAgentSpeed;
    }
}
