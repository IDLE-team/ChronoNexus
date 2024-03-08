using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MovableEntity : Entity
{
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private NavMeshAgent _navMeshAgent;

    private float _lastNavAcceleration;
    private float _lastNavSpeed;
    private float _lastNavAngularSpeed;
    
    public MovableEntityStateIdle IdleState { get; private set; }
    public MovableEntityStateRandomMove RandomMoveState { get; private set; }
    public MovableEntityStateChase ChaseState { get; private set; }

    [SerializeField]private Vector3[] _patrolPoints;
    public Vector3[] PatrolPoints => _patrolPoints;
    
    protected override void InitializeStartState()
    {
        Debug.Log("2");
        
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
            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
    }
    protected override void InitializeParam()
    {
        base.InitializeParam();
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        RandomMoveState = new MovableEntityStateRandomMove(this, _stateMachine);
        IdleState = new MovableEntityStateIdle(this, _stateMachine);
        ChaseState = new MovableEntityStateChase(this, _stateMachine);
    }

    protected override void OnDied()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.speed = 0;
        _navMeshAgent.angularSpeed = 0;

        _navMeshAgent.isStopped = true;
        base.OnDied();
    }

    protected override void Die()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _loot.DropLoot();

        base.Die();
    }

    public override void RealTimeAction()
    {
        base.RealTimeAction();
        if (gameObject != null)
        {
            if (_isAlive)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _lastNavSpeed;
                _navMeshAgent.angularSpeed = _lastNavAngularSpeed;
            }
        }
    }

    public override void StopTimeAction()
    {
        
        if (gameObject != null)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.speed = 0;
            _navMeshAgent.angularSpeed = 0;
            _navMeshAgent.velocity = Vector3.zero;
        }
        base.StopTimeAction();
    }

    public override void SlowTimeAction()
    {
        
        if (gameObject != null)
        {
            SetLastNavMeshValues();

            _navMeshAgent.speed = 0.1f;
            _navMeshAgent.angularSpeed = 0.1f;
        }
        base.SlowTimeAction();
    }

    public void StartMoveAnimation()
    {
        _animator.StartMoveAnimation();
    }

    public void EndMoveAnimation()
    {
        _animator.EndMoveAnimation();
    }

    public void StartAttackAnimation()
    {
        _animator.PlayAttackAnimation();
    }

    private void SetLastNavMeshValues()
    {
        _lastNavSpeed = _navMeshAgent.speed;
        _lastNavAngularSpeed = _navMeshAgent.angularSpeed;
    }
    
    public override void TargetFoundReaction()
    {
        _stateMachine.ChangeState(ChaseState);
    }
    
    public virtual void TargetChaseDistanceSwitch()
    {
        if (Vector3.Distance(SelfAim.position, Target.position) > 12f)//view distance or check last point
        {
            _stateMachine.ChangeState(RandomMoveState);
        }
    }
}