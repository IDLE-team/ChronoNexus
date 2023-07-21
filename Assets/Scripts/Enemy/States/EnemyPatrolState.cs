using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _destination;

    public EnemyPatrolState(Enemy _enemy, StateMachine StateMachine) : base(_enemy, StateMachine)
    {
    }

    public override void Enter()
    {
        _destination = GetRandomDirection();
        _enemy.navMeshAgent.SetDestination(_destination);
    }

    public override void LogicUpdate()
    {
        if (_enemy.canSeePlayer)
        {
            _enemy.StateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        if (_enemy.navMeshAgent.remainingDistance <= 1f)
        {
            _destination = GetRandomDirection();
            _enemy.navMeshAgent.SetDestination(_destination);
        }
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