using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemySoldierChaseState : EnemySoldierState
{
    //protected new EnemySoldier _enemy;
    protected Vector3 _targetPosision;
    
    protected float _maxChaseDistance = 15f;
    protected float _rangeAttackDistance = 8f;
    
    protected float _runningRangeAgentSpeed = 4f;


    public EnemySoldierChaseState(EnemySoldier enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
        
    }

    public override void Enter()
    {
        _targetPosision = _enemy.Target.position;

        if (!_enemy.isTimeSlowed && !_enemy.isTimeStopped)
        {
            _enemy.NavMeshAgent.speed = _enemy.EnemyAttacker.RangeAttackAgentSpeed;
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

        _targetPosision = _enemy.Target.position;
        _enemy.NavMeshAgent.SetDestination(_targetPosision);
    }

    private void SelectState()
    {
        if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosision) > _maxChaseDistance)
        {
            _stateMachine.ChangeState(_enemy.PatrolState);
        }

        /*if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosision) <= _meleeAttackDistance)
        {
            _stateMachine.ChangeState(_enemy.MeleeAttackState);
        }*/
         if (Vector3.Distance(_enemy.SelfAim.transform.position, _targetPosision) <= _rangeAttackDistance)
        {
            _stateMachine.ChangeState(_enemy.RangeAttackState);
        }
    }
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_enemy.isTimeSlowed && !_enemy.isTimeStopped);
        //_enemy.NavMeshAgent.speed *= 2;
        _enemy.NavMeshAgent.speed = _enemy.EnemyAttacker.RangeAttackAgentSpeed;
    }
}