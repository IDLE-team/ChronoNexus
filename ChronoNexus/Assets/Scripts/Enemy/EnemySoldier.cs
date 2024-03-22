using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class EnemySoldier : Enemy,ISeeker
{
    private EnemyLoot _loot;

    private bool _isLowHPBuffSelected;
    
    
    
    public  EnemySoldierPatrolState PatrolState{ get; private set; }
    public  EnemySoldierIdleState IdleState{ get; private set; }
    public EnemyRangeAttackState RangeAttackState { get; private set; }
    public  EnemySoldierChaseState ChaseState { get; private set; }
    public EnemyMeleeAttackState MeleeAttackState { get; private set; }
    public override string CurrentState
    {
        get
        {
            switch (_stateMachine.CurrentState)
            {
                case EnemyDummyState:
                    return State.Dummy.ToString();
                case EnemyIdleState:
                    return State.Idle.ToString();
                case EnemyPatrolState:
                    return State.Patrol.ToString();
                case EnemySoldierChaseState:
                    return State.Chase.ToString();
                case EnemyMeleeAttackState:
                    return State.Attack.ToString();
                default:
                    return " ";
            }
        }
    }
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public TargetFinder TargetFinder => _targetFinder;
    public EnemySoldierAttacker EnemyAttacker => _enemyAttacker;
    private  EnemySoldierAttacker _enemyAttacker;

    private TargetFinder _targetFinder;
    private NavMeshAgent _navMeshAgent;
    
    
    private float _lastNavAcceleration;
    private float _lastNavSpeed;
    private float _lastNavAngularSpeed;
    
    
    protected override void InitializeParam()
    {
        base.InitializeParam();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyAttacker = GetComponent<EnemySoldierAttacker>();
        _loot = GetComponent<EnemyLoot>();
        _targetFinder = GetComponent<TargetFinder>();
        
        IdleState = new EnemySoldierIdleState(this, _stateMachine);

        PatrolState = new EnemySoldierPatrolState(this, _stateMachine);
        ChaseState = new EnemySoldierChaseState(this, _stateMachine);

        //MeleeAttackState = new EnemyMeleeAttackState(this, _stateMachine);
        _enemyAttacker = GetComponent<EnemySoldierAttacker>();
        RangeAttackState = new EnemyRangeAttackState(this, _stateMachine);
    }

    protected virtual void Fire()
    {
        
    }
    public override void TakeDamage(float damage, bool isCritical)
    {
        if (!_isAlive)
            return;

        if (_stateMachine.CurrentState != DummyState && Target == null)
        {
            _navMeshAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }

        if (!_isLowHPBuffSelected && _health.Value <= _health.MaxHealth / 4 && _stateMachine.CurrentState != DummyState)
        {
            _isLowHPBuffSelected = true;

            float _chance = UnityEngine.Random.Range(1, 4);
            switch (_chance)
            {
                case 1:
                case 2:
                    Debug.Log("AggressionState");
                    break;
                case 3:
                    Debug.Log("FearState");
                    break;
                case 4:
                    Debug.Log("PrudenceState");
                    break;
            }
        }

        base.TakeDamage(damage, isCritical);
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
        _loot.DropItems();

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
        base.StopTimeAction();
        if (gameObject != null)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.speed = 0;
            _navMeshAgent.angularSpeed = 0;
            _navMeshAgent.velocity = Vector3.zero;
        }
    }

    public override void SlowTimeAction()
    {
        base.SlowTimeAction();
        if (gameObject != null)
        {
            SetLastNavMeshValues();

            _navMeshAgent.speed = 0.1f;
            _navMeshAgent.angularSpeed = 0.1f;
        }
    }

    public void MeleeAttack()
    {
        
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

    protected void SetLastNavMeshValues()
    {
        _lastNavSpeed = _navMeshAgent.speed;
        _lastNavAngularSpeed = _navMeshAgent.angularSpeed;
    }
}