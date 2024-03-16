using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MovableSoldierEntityStateAttack : MovableSoldierEntityState
{
    private FirearmWeapon _firearmWeapon;
    private Transform _target;
    private Vector3 _targetPosition;
    private float _minDelay;
    private float _maxDelay;
    private float _maxDistanceBetweenTarget = 8f;
    private float _minDistanceBetweenTarget;
    private bool _isAttack = false;
    private float _shootingAgentSpeed;
    private Vector3 _randomDirection;
    private Vector3 _retreatPosition;

    //Vector3 targetPosition;
    private Quaternion _toRotation;
    private CancellationTokenSource _cancellationTokenSource;

    public MovableSoldierEntityStateAttack(MovableSoldierEntity movableSoldierEntity, StateMachine stateMachine) : base(
        movableSoldierEntity, stateMachine)
    {
    }

    public override void Enter()
    { 
        base.Enter();
        Debug.Log(_movableSoldierEntity);
        Debug.Log(_movableSoldierEntity.SoldierAttacker);
        Debug.Log(_movableSoldierEntity.SoldierAttacker.MaxRangeAttackDistance);
        _maxDistanceBetweenTarget = _movableSoldierEntity.SoldierAttacker.MaxRangeAttackDistance;
        _minDistanceBetweenTarget = _movableSoldierEntity.SoldierAttacker.MinRangeDistanceToTarget;
        _minDelay = _movableSoldierEntity.SoldierAttacker.MinDelayTokenRange;
        _maxDelay = _movableSoldierEntity.SoldierAttacker.MaxDelayTokenRange;
        _shootingAgentSpeed = _movableSoldierEntity.SoldierAttacker.RangeAttackAgentSpeed;
        
        _movableSoldierEntity.Equiper.EquipWeapon(_movableSoldierEntity.SoldierAttacker.RangeWeaponData);
        
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

        _cancellationTokenSource = new CancellationTokenSource();
        ShootAndRetreat(_cancellationTokenSource.Token).Forget();
        
    }

    public override void Exit()
    {
        _isAttack = false;
        _movableSoldierEntity.IsTargetFound = false;
        _movableSoldierEntity.TargetFinder.SetWeight(0);
        _movableSoldierEntity.OnDie -= CancelCancelationToken;
        _firearmWeapon.OnReload -= ReloadingLogicEnter;
        _firearmWeapon.OnReloadEnd -= ReloadingLogicExit;
        if (!_movableSoldierEntity.isTimeSlowed && !_movableSoldierEntity.isTimeStopped)
            _movableSoldierEntity.NavMeshAgent.speed = 1.5f;
        _movableSoldierEntity.EndMoveAnimation();
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (_movableSoldierEntity.Target == null)
        {
            _cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_movableSoldierEntity.RandomMoveState);
            return;
        }

        _toRotation = Quaternion.LookRotation(_targetPosition - _movableSoldierEntity.transform.transform.position,
            Vector3.up);
        
        if (_movableSoldierEntity.isTimeSlowed)
        {
            TimeSlowedLogicUpdate();
        }
        else
        {
            TimeDefaultLogicUpdate();
        }

        if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) >
            _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_movableSoldierEntity.ChaseState);
            return;
        }

        Debug.Log("1 "+_movableSoldierEntity.transform);
        Debug.Log("2 "+_movableSoldierEntity.Target);
        _firearmWeapon.Fire(_movableSoldierEntity.Target, _movableSoldierEntity.transform);
        base.LogicUpdate();
        
    }

    private void TimeSlowedLogicUpdate()
    {
        if (_firearmWeapon != null && !_firearmWeapon.IsReloading)
        {
            _movableSoldierEntity.transform.rotation = Quaternion.Slerp(_movableSoldierEntity.transform.rotation,
                _toRotation, 6f * 0.2f * Time.deltaTime);
        }
    }

    private void TimeDefaultLogicUpdate()
    {
        if (_firearmWeapon != null && !_firearmWeapon.IsReloading)
        {
            _movableSoldierEntity.transform.rotation = Quaternion.Slerp(_movableSoldierEntity.transform.rotation,
                _toRotation, 6f * Time.deltaTime);
        }
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
            _cancellationTokenSource.Cancel();
            return;
        }

        _targetPosition = _movableSoldierEntity.Target.GetTransform().position;
        base.PhysicsUpdate();
    }

    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableSoldierEntity.isTimeSlowed && !_movableSoldierEntity.isTimeStopped);
        _movableSoldierEntity.NavMeshAgent.speed = _shootingAgentSpeed;
        //Default speed
    }

    private void CancelCancelationToken()
    {
        _cancellationTokenSource.RegisterRaiseCancelOnDestroy(_movableSoldierEntity.gameObject);
        _cancellationTokenSource.Cancel();
    }

    private async UniTask ShootAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int) (Random.Range(_minDelay, _maxDelay) * 1000));
            if (_movableSoldierEntity == null) _cancellationTokenSource.Cancel();
            if (_movableSoldierEntity.Target == null) _cancellationTokenSource.Cancel();
            if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) >
                _maxDistanceBetweenTarget) _cancellationTokenSource.Cancel();
            if (!cancellationToken.IsCancellationRequested && !_firearmWeapon.IsReloading &&
                Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) <=
                _maxDistanceBetweenTarget &&
                Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) >
                _minDistanceBetweenTarget)
            {
                do
                {
                    _randomDirection = Random.insideUnitSphere.normalized;
                    _retreatPosition = _movableSoldierEntity.SelfAim.transform.position +
                                       _randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    _retreatPosition = new Vector3(_retreatPosition.x, _movableSoldierEntity.SelfAim.position.y,
                        _retreatPosition.z);
                } while (Vector3.Distance(_targetPosition, _retreatPosition) < _minDistanceBetweenTarget ||
                         Vector3.Distance(_targetPosition, _retreatPosition) > _maxDistanceBetweenTarget);

                if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _retreatPosition) > 0.1f)
                {
                    _movableSoldierEntity.NavMeshAgent.SetDestination(_retreatPosition);
                    if (_movableSoldierEntity.isTimeSlowed) _movableSoldierEntity.NavMeshAgent.speed = 0.1f;
                    else _movableSoldierEntity.NavMeshAgent.speed = _movableSoldierEntity.SoldierAttacker.DefaultAgentSpeed;
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
            else if (!cancellationToken.IsCancellationRequested && !_firearmWeapon.IsReloading &&
                     Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _targetPosition) <=
                     _minDistanceBetweenTarget)
            {
                do
                {
                    _randomDirection = Random.insideUnitSphere.normalized;
                    _retreatPosition = _movableSoldierEntity.SelfAim.transform.position +
                                       _randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    _retreatPosition = new Vector3(_retreatPosition.x, _movableSoldierEntity.SelfAim.position.y,
                        _retreatPosition.z);
                } while (Vector3.Distance(_targetPosition, _retreatPosition) <= _minDistanceBetweenTarget);

                if (Vector3.Distance(_movableSoldierEntity.SelfAim.transform.position, _retreatPosition) > 0.1f)
                {
                    if (_movableSoldierEntity.isTimeStopped) return;
                    _movableSoldierEntity.NavMeshAgent.SetDestination(_retreatPosition);
                    if (_movableSoldierEntity.isTimeSlowed) _movableSoldierEntity.NavMeshAgent.speed = 0.2f;
                    else _movableSoldierEntity.NavMeshAgent.speed = _shootingAgentSpeed;
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
                _movableSoldierEntity.TargetFinder.SetWeight(0);
                _movableSoldierEntity.EndMoveAnimation();
                await UniTask.Yield();
            }
        }

        _movableSoldierEntity.TargetFinder.SetWeight(0);
        await UniTask.Yield();
    }
}