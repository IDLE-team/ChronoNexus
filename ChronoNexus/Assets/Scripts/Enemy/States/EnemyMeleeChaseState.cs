using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyMeleeChaseState : EnemyHumanoidState
{
    protected Vector3 _targetPosision;
    
    protected float _maxChaseDistance = 10f;
    protected float _meleeAttackDistance = 2f;
    
    protected float _runningMeleeAgentSpeed = 4f;


    public EnemyMeleeChaseState(EnemyHumanoid enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
        
    }

    public override void Enter()
    {
        _targetPosision = _enemy.Target.GetTransform().position;

        if (!_enemy.isTimeSlowed && !_enemy.isTimeStopped)
        {
            _enemy.NavMeshAgent.speed = _runningMeleeAgentSpeed;
        }
        else
        {
            TimeWaiter().Forget();
        }

        
        _enemy.StartMoveAnimation();
    }

    public override void Exit()
    {
        _enemy.IsTargetFound = false;
    }

    public override void LogicUpdate()
    {
        if (_enemy.Target == null)
        {
            _stateMachine.ChangeState(_enemy.IdleState);
            return;
        }

        SelectState();
    }

    public override void PhysicsUpdate()
    {
        if (_enemy.Target == null)
        {
            _stateMachine.ChangeState(_enemy.IdleState);
            return;
        }

        _targetPosision = _enemy.Target.GetTransform().position;
        _enemy.NavMeshAgent.SetDestination(_targetPosision);
    }

    protected virtual void SelectState()
    {
        if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosision) > _maxChaseDistance)
        {
            _stateMachine.ChangeState(_enemy.PatrolState);
        }

        if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosision) <= _meleeAttackDistance)
        {
            _stateMachine.ChangeState(_enemy.MeleeAttackState);
        }
    }
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_enemy.isTimeSlowed && !_enemy.isTimeStopped);
        //_enemy.NavMeshAgent.speed *= 2;
        _enemy.NavMeshAgent.speed = _runningMeleeAgentSpeed;
    }
}