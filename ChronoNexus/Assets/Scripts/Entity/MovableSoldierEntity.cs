using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MovableSoldierEntity : MovableMeleeEntity
{
    private EnemySoldierAttacker _enemySoldierAttacker;
    public EnemySoldierAttacker EnemySoldierAttacker => _enemySoldierAttacker;
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
        
        _enemySoldierAttacker = GetComponent<EnemySoldierAttacker>();
        RangeAttackState = new MovableSoldierEntityStateAttack(this, _stateMachine);
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

    
}