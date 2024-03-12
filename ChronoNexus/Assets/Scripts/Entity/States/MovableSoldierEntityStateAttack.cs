using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MovableSoldierEntityStateAttack : MovableSoldierEntityState
{
    private FirearmWeapon _firearmWeapon;
    
    private Transform _target;
    private Vector3 _targetPosition;

    private float shootingTimer = 0;
    private float shootingInterval = 1f;

    private float retreatDistance = 1f;
    private float minDelay = 0.5f;
    private float maxDelay = 2f;

    private float reloadTimer = 0;
    private float reloadInterval = 3f;

   // private bool _isReloading = false;

    private int ammoCount = 10;
    private int ammoMaxCount = 10;

    private float _maxDistanceBetweenTarget = 8f;
    private float _minDistanceBetweenTarget = 5f;

    private bool _isAttack = false;

    private float _defaultAgentSpeed = 1.5f;
    private float _runningAgentSpeed = 2f;

    Vector3 randomDirection;
    Vector3 retreatPosition;
    //Vector3 targetPosition;
    Quaternion toRotation;


    private CancellationTokenSource cancellationTokenSource;

    public MovableSoldierEntityStateAttack(MovableSoldierEntity movableSoldierEntity, StateMachine stateMachine) : base(movableSoldierEntity, stateMachine)
    {
        //_remainingDistance = _entity.EntityLogic.RemainingDistanceToRandomPosisiton
    }

    public override void Enter()
    {
        //shootingInterval = _movableSoldierEntityState.EnemyAttacker.RangedAttackInterval;
        //ammoMaxCount = _movableSoldierEntityState.EnemyAttacker._AmmoCount;
        ammoCount = ammoMaxCount;
        //_enemy.NavMeshAgent.speed = 2.5f;
        _movableSoldierEntity.Equiper.EquipWeapon(_movableSoldierEntity.EnemySoldierAttacker.RangeWeaponData);
        if (_movableSoldierEntity.WeaponController.CurrentWeapon.WeaponType == WeaponType.Firearm)
        {
            _firearmWeapon = (FirearmWeapon)_movableSoldierEntity.WeaponController.CurrentWeapon;
        }
        _target = _movableSoldierEntity.Target.GetTransform();
        _targetPosition = _target.localPosition;
        _firearmWeapon.OnReload += ReloadingLogicEnter;
        _firearmWeapon.OnReloadEnd += ReloadingLogicExit;
        _movableSoldierEntity.TargetFinder.SetWeight(1);

        _isAttack = true;
        _movableSoldierEntity.OnDie += CancelCancelationToken;
        cancellationTokenSource = new CancellationTokenSource();
        ShootAndRetreat(cancellationTokenSource.Token).Forget();
        base.Enter();
    }

    public override void Exit()
    {
        _isAttack = false;
        _movableSoldierEntity.IsTargetFound = false;
        _movableSoldierEntity.TargetFinder.SetWeight(0);
        _movableSoldierEntity.OnDie -= CancelCancelationToken;
        _firearmWeapon.OnReload -= ReloadingLogicEnter;
        _firearmWeapon.OnReloadEnd -= ReloadingLogicExit;
        if(!_movableSoldierEntity.isTimeSlowed && !_movableSoldierEntity.isTimeStopped)
            _movableSoldierEntity.NavMeshAgent.speed = 1.5f;
        _movableSoldierEntity.EndMoveAnimation();
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (_movableSoldierEntity.Target == null)
        {
            cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_movableSoldierEntity.PatrolState);
            return;
        }
        
        toRotation = Quaternion.LookRotation(_targetPosition - _movableSoldierEntity.transform.transform.position, Vector3.up);

        if (!_firearmWeapon.IsReloading)
        {
            _movableSoldierEntity.transform.rotation = Quaternion.Slerp(_movableSoldierEntity.transform.rotation,toRotation, 6f * Time.deltaTime);
        }
        if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_movableSoldierEntity.ChaseState);
            return;
        }
        
        _firearmWeapon.Fire(_movableSoldierEntity.Target, _movableSoldierEntity.transform);
        
        base.LogicUpdate();
        /*
        if (_movableSoldierEntity.Target == null)
        {
            cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_movableSoldierEntity.PatrolState);
            return;
        }

        toRotation = Quaternion.LookRotation(_targetPosition - _movableSoldierEntity.transform.transform.position, Vector3.up);
        if (_movableSoldierEntity.isTimeSlowed)
        {
            if(!_isReloading)
            {
                _movableSoldierEntity.transform.rotation = Quaternion.Slerp(_movableSoldierEntity.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
            }
        }
        else
        {
            if(!_isReloading)
            {
                _movableSoldierEntity.transform.rotation = Quaternion.Slerp(_movableSoldierEntity.transform.rotation, toRotation, 6f * Time.deltaTime);
            }
        }

        if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_movableSoldierEntity.ChaseState);
            return;
        }

        if (!_isReloading)
        {
            if (_movableSoldierEntity.isTimeSlowed)
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
            _movableSoldierEntity.EnemySoldierAttacker.Shoot(_targetPosition);
            shootingTimer = shootingInterval;
            if (ammoCount > 0)
            {
                ammoCount--;
            }
            else if (!_isReloading)
            {
                ammoCount = ammoMaxCount;
                reloadTimer = reloadInterval;
                _movableSoldierEntity.TargetFinder.SetWeight(0);
                _movableSoldierEntity.EndMoveAnimation();
                _movableSoldierEntity.NavMeshAgent.SetDestination(_movableSoldierEntity.SelfAim.transform.position);
                //start reloading animation
                _isReloading = true;
            }
        }
        if (reloadTimer >= 0f && _isReloading)
        {
            if (_movableSoldierEntity.isTimeSlowed)
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
            _movableSoldierEntity.StartMoveAnimation();
            _movableSoldierEntity.TargetFinder.SetWeight(1);
            _isReloading = false;
        }

        base.LogicUpdate();
        */
    }

    private void ReloadingLogicEnter()
    {
        _movableSoldierEntity.NavMeshAgent.SetDestination(_movableSoldierEntity.SelfAim.transform.position);
        _movableSoldierEntity.TargetFinder.SetWeight(0);
        _movableSoldierEntity.EndMoveAnimation();
    }

    private void ReloadingLogicExit()
    {
        _movableSoldierEntity.TargetFinder.SetWeight(1);
        _movableSoldierEntity.StartMoveAnimation();

    }

    
    public override void PhysicsUpdate()
    {
        if (_movableSoldierEntity.Target == null)
        {
            cancellationTokenSource.Cancel();
            return;
        }

        _targetPosition = _movableSoldierEntity.Target.GetTransform().position;
        base.PhysicsUpdate();
    }

    private void CancelCancelationToken()
    {
        cancellationTokenSource.RegisterRaiseCancelOnDestroy(_movableSoldierEntity.gameObject);
        cancellationTokenSource.Cancel();
    }
    private async UniTask ShootAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int)(Random.Range(minDelay, maxDelay) * 1000));

            if (_movableSoldierEntity == null)
                cancellationTokenSource.Cancel();
            if (_movableSoldierEntity.Target == null)
                cancellationTokenSource.Cancel();
            if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();


            if (!cancellationToken.IsCancellationRequested
                && !_firearmWeapon.IsReloading
                && Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) <= _maxDistanceBetweenTarget
                && Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) > _minDistanceBetweenTarget
                )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _movableSoldierEntity.SelfAim.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _movableSoldierEntity.SelfAim.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) < _minDistanceBetweenTarget || Vector3.Distance(_targetPosition, retreatPosition) > _maxDistanceBetweenTarget);


                if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, retreatPosition) > 0.1f)
                {
                    _movableSoldierEntity.NavMeshAgent.SetDestination(retreatPosition);
                    
                    if(_movableSoldierEntity.isTimeSlowed)
                        _movableSoldierEntity.NavMeshAgent.speed = 0.1f;
                    else
                        _movableSoldierEntity.NavMeshAgent.speed = _defaultAgentSpeed;
                    
                    _movableSoldierEntity.StartMoveAnimation();
                }
                else
                {
                    _movableSoldierEntity.NavMeshAgent.SetDestination(_movableSoldierEntity.SelfAim.transform.position);
                    _movableSoldierEntity.EndMoveAnimation();
                }
                _movableSoldierEntity.TargetFinder.SetWeight(1);
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested
                 && !_firearmWeapon.IsReloading
                 && Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) <= _minDistanceBetweenTarget

                 )
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _movableSoldierEntity.SelfAim.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _movableSoldierEntity.SelfAim.transform.position.y, retreatPosition.z);

                } while (Vector3.Distance(_targetPosition, retreatPosition) <= _minDistanceBetweenTarget);


                if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, retreatPosition) > 0.1f)
                {
                    if (_movableSoldierEntity.isTimeStopped)
                        return;
                    _movableSoldierEntity.NavMeshAgent.SetDestination(retreatPosition);
                    
                    if (_movableSoldierEntity.isTimeSlowed)
                        _movableSoldierEntity.NavMeshAgent.speed = 0.2f;
                    else
                        _movableSoldierEntity.NavMeshAgent.speed = _runningAgentSpeed;
                    
                    _movableSoldierEntity.StartMoveAnimation();
                }
                else
                {
                    _movableSoldierEntity.NavMeshAgent.SetDestination(_movableSoldierEntity.SelfAim.transform.position);
                    _movableSoldierEntity.EndMoveAnimation();
                }
                _movableSoldierEntity.TargetFinder.SetWeight(1);
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested && _firearmWeapon.IsReloading)
            {
                _movableSoldierEntity.NavMeshAgent.SetDestination(_movableSoldierEntity.SelfAim.transform.position);
              //  _movableSoldierEntity.NavMeshAgent.speed = _defaultAgentSpeed;
                _movableSoldierEntity.TargetFinder.SetWeight(0);
                _movableSoldierEntity.EndMoveAnimation();
                await UniTask.Yield();
            }
        }
        _movableSoldierEntity.TargetFinder.SetWeight(0);
        await UniTask.Yield();
    }
}