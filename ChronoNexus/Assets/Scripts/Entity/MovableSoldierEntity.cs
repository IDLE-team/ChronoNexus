using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MovableSoldierEntity : MovableMeleeEntity
{
    private MovableEntitySoldierAttacker _soldierAttacker;
    public MovableEntitySoldierAttacker SoldierAttacker => _soldierAttacker;
    public MovableSoldierEntityStateAttack RangeAttackState { get; private set; }
    protected override void InitializeStartState()
    {
        switch (state)
        {
            case State.Dummy:
                _stateMachine.Initialize(DummyState);
                break;
            case State.Idle:
                _stateMachine.Initialize(IdleState);
                break;
            case State.RandomMove:
                _stateMachine.Initialize(RandomMoveState);
                break;
            case State.Patrol:
                _stateMachine.Initialize(PatrolState);
                break;
            case State.Chase:
                _stateMachine.Initialize(ChaseState);
                break;
            case State.MeleeAttack:
                _stateMachine.Initialize(MeleeAttackState);
                break;
            case State.RangeAttack:
                _stateMachine.Initialize(RangeAttackState);
                break;
            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
    }
    protected override void InitializeParam()
    {
        base.InitializeParam();
       
        RangeAttackState = new MovableSoldierEntityStateAttack(this, _stateMachine);
    }
    protected override void InitializeIndividualParam()
    {
         _soldierAttacker = GetComponent<MovableEntitySoldierAttacker>();
    }
    public override void TargetChaseDistanceSwitch()
    {
        if (Vector3.Distance(SelfAim.position, Target.GetTransform().position) > 12f) //view distance or check last point
        {
            _stateMachine.ChangeState(RandomMoveState);
        }
        else if(Vector3.Distance(SelfAim.position, Target.GetTransform().position) <= 8f) // or attack range
        {
            _stateMachine.ChangeState(RangeAttackState);
        }
    }
    public override void AgentDestinationSet()
    {
        if (Vector3.Distance(SelfAim.position, Target.GetTransform().position) > 8f)
        {
            _navMeshAgent.SetDestination(Target.GetTransform().position);
        }
    }
    public override void StopTimeAction()
    {
        if (gameObject != null)
        {
            if (WeaponController.CurrentWeapon)
            {
                if (WeaponController.CurrentWeapon.WeaponType == WeaponType.Firearm)
                {
                    FirearmWeapon _firearmWeapon = (FirearmWeapon) WeaponController.CurrentWeapon;
                    _firearmWeapon.StopFire();
                }
            }
        }
        base.StopTimeAction();
    }

    public override void SlowTimeAction()
    {
        if (gameObject != null)
        {
            if (WeaponController.CurrentWeapon.WeaponType == WeaponType.Firearm)
            {
                FirearmWeapon _firearmWeapon = (FirearmWeapon) WeaponController.CurrentWeapon;

            }
        }
        base.SlowTimeAction();
    }
    protected override void Die()
    {
        if (WeaponController.CurrentWeapon != null)
        {
            if (WeaponController.CurrentWeapon.WeaponType == WeaponType.Firearm)
            {
                FirearmWeapon _firearmWeapon = (FirearmWeapon) WeaponController.CurrentWeapon;
                _firearmWeapon.StopFire();
            }
        }

        base.Die();
    }

    
}