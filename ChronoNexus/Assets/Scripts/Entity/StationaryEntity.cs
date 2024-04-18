using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StationaryEntity : Entity
{
    [SerializeField]private GameObject smokeVFX;
    [SerializeField]private GameObject fireVFX;
    
    private StationaryEntityAttacker _attacker;
    public StationaryEntityAttacker TurretAttacker => _attacker;

    public StationaryEntityStateIdle IdleState { get; private set; }

    public StationaryEntityStateRangeAttack RangeAttackState { get; private set; }
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
                case State.RangeAttack:
                    _stateMachine.Initialize(RangeAttackState);
                break;
            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
    }

    protected override void Die()
    {
        smokeVFX.SetActive(true);
        fireVFX.SetActive(true);
        base.Die();
    }

    protected override void InitializeParam()
    {
        base.InitializeParam();
        RangeAttackState = new StationaryEntityStateRangeAttack(this, _stateMachine);
        IdleState = new StationaryEntityStateIdle(this, _stateMachine);
    }

    protected override void InitializeIndividualParam()
    {
        _attacker = GetComponent<StationaryEntityAttacker>();
    }
    public override void TargetFoundReaction()
    {
        _stateMachine.ChangeState(RangeAttackState);
    }
}