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
    private float _maxAttackDistance = 12f;
    private float _minAttackDistance;
    private bool _isAttack = false;

    private float shootingTimer = 0;
    private float shootingInterval;

    private float retreatDistance = 1f;

    private float reloadTimer = 0;
    private float reloadInterval = 3f;

    private float _setTargetOnAim = 0.5f;
    private float _setTargetOnAimTemp = 0.5f;

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
        Debug.Log("RangeStatEnterState");
        
        _setTargetOnAimTemp = _setTargetOnAim;
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
        _cancellationTokenSource = new CancellationTokenSource();
        _stationaryEntity.OnDie += CancelCancelationToken;
        base.Enter();
    }

    public override void Exit()
    {        Debug.Log("RangeStatExitState");

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
            Debug.Log("Target");
            _stateMachine.ChangeState(_stationaryEntity.IdleState);
            return;
        }

        Vector3 selfPos = new Vector3(_stationaryEntity.transform.position.x, _targetPosition.y,
            _stationaryEntity.transform.position.z);
        _toRotation = Quaternion.LookRotation(_targetPosition - selfPos, Vector3.up);
        if (_stationaryEntity.isTimeSlowed)
        {
            _stationaryEntity.transform.rotation = Quaternion.Slerp(_stationaryEntity.transform.rotation, _toRotation, 6f * 0.2f * Time.deltaTime);
        }
        else
        {
            _stationaryEntity.transform.rotation = Quaternion.Slerp(_stationaryEntity.transform.rotation, _toRotation, 6f * Time.deltaTime);
        }

        if (Quaternion.Angle(_stationaryEntity.transform.rotation, _toRotation) <
            10f) 
        {
            ShootLogic();
        }

        ReloadLogic();

        if (Vector3.Distance(_stationaryEntity.SelfAim.transform.position, _targetPosition) > _maxAttackDistance)
        {
            Debug.Log("DistanceExit");
            _stateMachine.ChangeState(_stationaryEntity.IdleState);
            return;
        }

        //_firearmWeapon.Fire(_stationaryEntity.Target, _stationaryEntity.transform);


        base.LogicUpdate();

    }

    private void ShootLogic()
    {
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

        
    }

    private void ReloadLogic()
    {
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