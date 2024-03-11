using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyPatrolState : EnemyHumanoidState
{
    private Vector3 _destination;
    private float _maxDistance = 10f;
    public EnemyPatrolState(EnemyHumanoid enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
        
    }

    public override void Enter()
    {
        if (!_enemy.isTimeSlowed && !_enemy.isTimeStopped)
        {
            //_enemy.NavMeshAgent.speed *= 2;
            _enemy.NavMeshAgent.speed = _enemy.EnemyAttacker.DefaultAgentSpeed;
        }
        else
        {
            TimeWaiter().Forget();
        }
        _enemy.StartSeek();
        _destination = GetRandomDirection();
        _enemy.NavMeshAgent.SetDestination(_destination);
        _enemy.StartMoveAnimation();
    }

    public override void LogicUpdate()
    {
        if (_enemy.IsTargetFound)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        if (!(_enemy.NavMeshAgent.remainingDistance <= 1f))
        {
            return;
        }
        _enemy.EndMoveAnimation();
        _destination = GetRandomDirection();
        _enemy.NavMeshAgent.SetDestination(_destination);
        _enemy.StartMoveAnimation();
    }

    private Vector3 GetRandomDirection()
    {
        return _enemy.transform.position + new Vector3(Random.Range(-_maxDistance, _maxDistance), 0f, Random.Range(-_maxDistance, _maxDistance));
    }

    public override void Exit()
    {
        _enemy.EndMoveAnimation();
        _enemy.StopSeek();
    }

    public override void PhysicsUpdate()
    {
        
    }
}