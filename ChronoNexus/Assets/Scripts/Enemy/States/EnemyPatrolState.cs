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
        enemy._navMeshAgent.SetDestination(_destination);
    }

    public override void LogicUpdate()
    {
        if (enemy.canSeePlayer)
        {
            enemy.enemySM.ChangeState(enemy.ChaseState);
            return;
        }

        if (enemy._navMeshAgent.remainingDistance <= 1f)
        {
            _destination = GetRandomDirection();
            enemy._navMeshAgent.SetDestination(_destination);
        }
    }

    private Vector3 GetRandomDirection()
    {
        return enemy.transform.position + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }

    public override void Exit()
    {
    }

    public override void PhysicsUpdate()
    {
    }
}