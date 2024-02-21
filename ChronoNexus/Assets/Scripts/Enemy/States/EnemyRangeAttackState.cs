using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class EnemyRangeAttackState : EnemyState
{
    private Transform _target;
    private Vector3 _targetPosition;

    private float shootingTimer = 0;
    private float shootingInterval;

    private float retreatDistance = 5f;
    private float minDelay = 3f;
    private float maxDelay = 4f;

    private float reloadTimer = 0;
    private float reloadInterval = 7f;

    private bool _isAttack = false;
    private bool _isReloading = false;

    private int ammoCount;
    private int ammoMaxCount;


    private CancellationTokenSource cancellationTokenSource;

    public EnemyRangeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        shootingInterval = _enemy.EnemyAttacker.RangedAttackInterval;
        ammoMaxCount = _enemy.EnemyAttacker._AmmoCount;
        ammoCount = ammoMaxCount;

        /*switch (_enemy.enemyType)
        {
            case Enemy.EnemyType.Stormtrooper:
                break;
            case Enemy.EnemyType.Guard:

                break;
            case Enemy.EnemyType.Juggernaut:
                break;
            default:
                break;
        }*/

        _enemy.NavMeshAgent.speed = 2.5f;

        _target = _enemy.Target.transform;

        _isAttack = true;

        cancellationTokenSource = new CancellationTokenSource();
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

        Vector3 targetPosition = _targetPosition - _enemy.transform.transform.position;


        // new Vector3(_targetPosition.x, _enemy.transform.position.y, _targetPosition.z);
        Quaternion toRotation = Quaternion.LookRotation(targetPosition, Vector3.up);
        //_enemy.transform.LookAt(targetPosition);
        if (_enemy.isTimeSlowed)
        {
            if (_enemy.enemyType != Enemy.EnemyType.Juggernaut)
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
            }
            else if(!_isReloading)
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
            }
        }
        else
        {
            if (_enemy.enemyType != Enemy.EnemyType.Juggernaut )
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * Time.deltaTime);
            }
            else if (!_isReloading)
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * Time.deltaTime);
            }
        }

        if (Vector3.Distance(_enemy.transform.position, _targetPosition) > 8f)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }
        if (_enemy.enemyType != Enemy.EnemyType.Juggernaut)
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0f)
            {
                _enemy.Shoot(_targetPosition);
                shootingTimer = shootingInterval;
            }
            return;
        }

        if (!_isReloading)
        {
            shootingTimer -= Time.deltaTime;
        }

        if (shootingTimer <= 0f && !_isReloading)
        {
            _enemy.EndMoveAnimation();
            _enemy.Shoot(_targetPosition);
            shootingTimer = shootingInterval;
            if(ammoCount > 0)
            {
                ammoCount--;
                
            }
            else
            {
                ammoCount = ammoMaxCount;
                reloadTimer = reloadInterval;
                _isReloading = true;
            }
        }
        if (reloadTimer >= 0f && _isReloading)
        {
            reloadTimer -= Time.deltaTime;
        }
        else
        {
            _enemy.StartMoveAnimation();
            _isReloading = false;
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
            if (_enemy.enemyType == Enemy.EnemyType.Juggernaut 
                //&& !_isReloading && ammoCount > 0
                )
            {
                cancellationTokenSource.Cancel();
            }

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