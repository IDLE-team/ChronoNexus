using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _destination;

    public EnemyPatrolState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        _destination = GetRandomDirection();
        _enemy.NavMeshAgent.SetDestination(_destination);
    }

    public override void LogicUpdate()
    {
        if (_enemy.canSeeTarget)
        {
            _stateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        if (!(_enemy.NavMeshAgent.remainingDistance <= 1f))
            return;
        _destination = GetRandomDirection();
        _enemy.NavMeshAgent.SetDestination(_destination);
    }

    private Vector3 GetRandomDirection()
    {
        return _enemy.transform.position + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }

    public override void Exit()
    {
    }

    public override void PhysicsUpdate()
    {
    }
}