using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MovableMeleeEntity : MovableEntity
{
    public MovableMeleeEntityStateAttack MeleeAttackState { get; private set; }
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
            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
    }
    protected override void InitializeParam()
    {
        base.InitializeParam();
        
        MeleeAttackState = new MovableMeleeEntityStateAttack(this, _stateMachine);
    }
    public void StartAttackAnimation()
    {
        _animator.PlayAttackAnimation();
    }
    public override void TargetChaseDistanceSwitch()
    {
        if (Vector3.Distance(SelfAim.position, Target.GetTransform().position) > 12f) //view distance or check last point
        {
            _stateMachine.ChangeState(RandomMoveState);
        }
        else if(Vector3.Distance(SelfAim.position, Target.GetTransform().position) <= 1.5f) // or attack range
        {
            _stateMachine.ChangeState(MeleeAttackState);
        }
    }
    public override void AgentDestinationSet()
    {
        if (Vector3.Distance(SelfAim.position, Target.GetTransform().position) > 1.5f)
        {
            _navMeshAgent.SetDestination(Target.GetTransform().position);
        }
    }

    public void MeleeAttack()
    {
        
    }
    
}