using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class JuggernautRangeAttackState : EnemyState
{
    private Transform _target;
    private Vector3 _targetPosition;

    private float shootingTimer = 1;
    private float shootingInterval;

    private float retreatDistance = 3f;
    private float minDelay = 0.5f;
    private float maxDelay = 2f;

    private float reloadTimer = 0;
    private float reloadInterval = 7f;

    private bool _isReloading = false;

    private int ammoCount;
    private int ammoMaxCount;

    private float _maxDistanceBetweenTarget = 12f;
    private float _minDistanceBetweenTarget = 7f;

    private bool _isAttack = false;

    private float _defaultAgentSpeed = 1.5f;
    private float _shootingAgentSpeed = 0.5f;

    Vector3 randomDirection;
    Vector3 retreatPosition;
    //Vector3 targetPosition;
    Quaternion toRotation;



    private CancellationTokenSource cancellationTokenSource;

    public JuggernautRangeAttackState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {

    }

    public override void Enter()
    {
        shootingInterval = _enemy.EnemyAttacker.RangedAttackInterval;
        ammoMaxCount = _enemy.EnemyAttacker._AmmoCount;
        ammoCount = ammoMaxCount;

        //_enemy.NavMeshAgent.speed = 0.75f;

        _target = _enemy.Target.transform;
        _targetPosition = _target.localPosition;
        Debug.Log("TargetInMethod: " + _targetPosition);

        _enemy.TargetFinder.SetWeight(1);
        _isAttack = true;

        _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
        _enemy.EndMoveAnimation();

        cancellationTokenSource = new CancellationTokenSource();
        ShootAndRetreat(cancellationTokenSource.Token).Forget();
    }

    public override void Exit()
    {
        _isAttack = false;
        _enemy.IsTargetFound = false;
        if(!_enemy.isTimeSlowed && !_enemy.isTimeStopped)
            _enemy.NavMeshAgent.speed = 1.5f;
        _enemy.TargetFinder.SetWeight(0);
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


        toRotation = Quaternion.LookRotation(_targetPosition - _enemy.transform.transform.position, Vector3.up);

        if (_enemy.isTimeSlowed)
        {
            if (!_isReloading)
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
            }
        }
        else
        {
            if (!_isReloading)
            {
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, toRotation, 6f * Time.deltaTime);
            }
        }

        if (Vector3.Distance(_enemy.transform.position, _targetPosition) >= _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        if (!_isReloading)
        {
            if (_enemy.isTimeSlowed)
            {
                shootingTimer -= (Time.deltaTime * 0.2f);
            }
            else
            {
                shootingTimer -= Time.deltaTime;
            }

        }

        if (shootingTimer <= 0f && !_isReloading)
        {

            _enemy.EnemyAttacker.Shoot(_targetPosition);
            shootingTimer = shootingInterval;
            if (ammoCount > 0)
            {
                ammoCount--;
            }
            else if (!_isReloading)
            {
                ammoCount = ammoMaxCount;
                _enemy.TargetFinder.SetWeight(0);
                reloadTimer = reloadInterval;
                _enemy.EndMoveAnimation();
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                //start reloading animation
                _isReloading = true;
            }
        }
        if (reloadTimer >= 0f && _isReloading)
        {
            if (_enemy.isTimeSlowed)
            {
                reloadTimer -= (Time.deltaTime * 0.2f);
            }
            else
            {
                reloadTimer -= Time.deltaTime;
            }
        }
        else if (_isReloading)
        {
            //stop reloading animation
            //_enemy.StartMoveAnimation();
            _enemy.TargetFinder.SetWeight(1);
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
            if (Vector3.Distance(_enemy.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();


            if (!cancellationToken.IsCancellationRequested
                && !_isReloading
                && Vector3.Distance(_enemy.transform.position, _targetPosition) <= _maxDistanceBetweenTarget
                && Vector3.Distance(_enemy.transform.position, _targetPosition) > _minDistanceBetweenTarget
                )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _enemy.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _enemy.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) < _minDistanceBetweenTarget || Vector3.Distance(_targetPosition, retreatPosition) > _maxDistanceBetweenTarget);


                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    if (_enemy.isTimeSlowed)
                        _enemy.NavMeshAgent.speed = 0.1f;
                    else
                    _enemy.NavMeshAgent.speed = _shootingAgentSpeed;
                    if(Vector3.Distance(_enemy.transform.position, _targetPosition) <= Vector3.Distance(retreatPosition, _targetPosition))
                    {
                        //back
                        _enemy.SetMoveX(0);
                    }
                    else
                    {
                        _enemy.SetMoveX(1);
                        //forward
                    }
                    _enemy.StartMoveAnimation();
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                    _enemy.EndMoveAnimation();
                }
                _enemy.TargetFinder.SetWeight(1);
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested
                 && !_isReloading
                 && Vector3.Distance(_enemy.transform.position, _targetPosition) <= _minDistanceBetweenTarget
                 )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _enemy.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _enemy.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) <= _minDistanceBetweenTarget);

                

                if (Vector3.Distance(_enemy.transform.position, retreatPosition) > 0.1f)
                {
                    _enemy.NavMeshAgent.SetDestination(retreatPosition);
                    if (_enemy.isTimeSlowed)
                        _enemy.NavMeshAgent.speed = 0.1f;
                    else
                    _enemy.NavMeshAgent.speed = _shootingAgentSpeed;
                    if (Vector3.Distance(_enemy.transform.position, _targetPosition) <= Vector3.Distance(retreatPosition, _targetPosition))
                    {
                        //back
                        _enemy.SetMoveX(0);
                    }
                    else
                    {
                        _enemy.SetMoveX(1);
                        //forward
                    }
                    _enemy.StartMoveAnimation();
                }
                else
                {
                    _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                    _enemy.EndMoveAnimation();
                }
                /*if(Vector3.Distance(_targetPosition, _enemy.transform.position) <= 2)
                {
                    _enemy.atta
                }
                else
                {*/
                    _enemy.TargetFinder.SetWeight(1);
                
                
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested && _isReloading)
            {
                _enemy.NavMeshAgent.SetDestination(_enemy.transform.position);
                if (_enemy.isTimeSlowed)
                    _enemy.NavMeshAgent.speed = 0.1f;
                else
                _enemy.NavMeshAgent.speed = _defaultAgentSpeed;
                _enemy.EndMoveAnimation();
                _enemy.TargetFinder.SetWeight(0);
                await UniTask.Yield();
            }

        }
        await UniTask.Yield();
    }
}
