using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _destination;

    public EnemyPatrolState(Enemy enemy, StateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override void Enter()
    {
        _enemy.StartSeek();
        _destination = GetRandomDirection();
        _enemy.NavMeshAgent.SetDestination(_destination);
    }

    public override void LogicUpdate()
    {
        if (_enemy.IsTargetFound)
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
        _enemy.StopSeek();
    }

    public override void PhysicsUpdate()
    {
    }
}