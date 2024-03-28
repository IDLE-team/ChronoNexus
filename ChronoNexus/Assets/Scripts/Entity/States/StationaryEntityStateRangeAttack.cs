using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StationaryEntityStateRangeAttack : StationaryEntityState
{
    private FirearmWeapon _firearmWeapon;

    private Transform _target;
    private Vector3 _targetPosition;
    private float _maxAttackDistance = 8f;
    private float _minAttackDistance;
    private bool _isAttack = false;

    private float shootingTimer = 0;
    private float shootingInterval;

    private float retreatDistance = 1f;

    private float reloadTimer = 0;
    private float reloadInterval = 3f;

    private bool _isReloading = false;

    private int ammoCount;
    private int ammoMaxCount;

    //Vector3 targetPosition;
    private Quaternion _toRotation;
    private CancellationTokenSource _cancellationTokenSource;

    public StationaryEntityStateRangeAttack(StationaryEntity stationaryEntity, StateMachine stateMachine) : base(
        stationaryEntity, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        shootingInterval = _stationaryEntity.TurretAttacker.RangedAttackInterval;
        ammoMaxCount = _stationaryEntity.TurretAttacker.AmmoCount;
        ammoCount = ammoMaxCount;

        // _stationaryEntity.Equiper.EquipWeapon(_stationaryEntity.TurretAttacker.RangeWeaponData);

        /*if (_stationaryEntity.WeaponController.CurrentWeapon.WeaponType == WeaponType.Firearm)
        {
            _firearmWeapon = (FirearmWeapon) _stationaryEntity.WeaponController.CurrentWeapon;
        }*/

        /*_firearmWeapon.OnReload += ReloadingLogicEnter;
        _firearmWeapon.OnReloadEnd += ReloadingLogicExit;
        */

        _target = _stationaryEntity.Target.GetTransform();
        _targetPosition = _target.localPosition;

        _isAttack = true;

        _stationaryEntity.OnDie += CancelCancelationToken;
    }

    public override void Exit()
    {
        _isAttack = false;
        _stationaryEntity.IsTargetFound = false;
        _stationaryEntity.OnDie -= CancelCancelationToken;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (_stationaryEntity.Target == null)
        {
            _cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_stationaryEntity.IdleState);
            return;
        }

        Vector3 selfPos = new Vector3(_stationaryEntity.transform.position.x, _targetPosition.y,
            _stationaryEntity.transform.position.z);
        _toRotation = Quaternion.LookRotation(_targetPosition - selfPos, Vector3.up);

        if (_stationaryEntity.isTimeSlowed)
        {
            _stationaryEntity.transform.rotation = Quaternion.Slerp(_stationaryEntity.transform.rotation,
                _toRotation, 6f * 0.2f * Time.deltaTime);
            //TimeSlowedLogicUpdate();
        }
        else
        {
            _stationaryEntity.transform.rotation = Quaternion.Slerp(_stationaryEntity.transform.rotation,
                _toRotation, 6f * Time.deltaTime);
            //TimeDefaultLogicUpdate();
        }

        if (Vector3.Distance(_stationaryEntity.SelfAim.transform.position, _targetPosition) > _maxAttackDistance)
        {
            _stateMachine.ChangeState(_stationaryEntity.IdleState);
            return;
        }

        //_firearmWeapon.Fire(_stationaryEntity.Target, _stationaryEntity.transform);

        if (!_isReloading)
        {
            if (_stationaryEntity.isTimeSlowed)
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
            _stationaryEntity.TurretAttacker.Shoot(_targetPosition);
            shootingTimer = shootingInterval;
            if (ammoCount > 0)
            {
                ammoCount--;
            }
            else if (!_isReloading)
            {
                ammoCount = ammoMaxCount;
                reloadTimer = reloadInterval;
                //start reloading animation
                _isReloading = true;
            }
        }

        if (reloadTimer >= 0f && _isReloading)
        {
            if (_stationaryEntity.isTimeSlowed)
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
            _isReloading = false;
        }

        base.LogicUpdate();

    }

    private void TimeSlowedLogicUpdate()
    {
        if (_firearmWeapon != null && !_firearmWeapon.IsReloading)
        {
            _stationaryEntity.transform.rotation = Quaternion.Slerp(_stationaryEntity.transform.rotation,
                _toRotation, 6f * 0.2f * Time.deltaTime);
        }
    }

    private void TimeDefaultLogicUpdate()
    {
        if (_firearmWeapon != null && !_firearmWeapon.IsReloading)
        {
            _stationaryEntity.transform.rotation = Quaternion.Slerp(_stationaryEntity.transform.rotation,
                _toRotation, 6f * Time.deltaTime);
        }
    }

    private void ReloadingLogicEnter()
    {

    }

    private void ReloadingLogicExit()
    {

    }

    public override void PhysicsUpdate()
    {
        if (_stationaryEntity.Target == null)
        {
            _cancellationTokenSource.Cancel();
            return;
        }

        _targetPosition = _stationaryEntity.Target.GetTransform().position;
        base.PhysicsUpdate();
    }

    private void CancelCancelationToken()
    {
        _cancellationTokenSource.RegisterRaiseCancelOnDestroy(_stationaryEntity.gameObject);
        _cancellationTokenSource.Cancel();
    }
}