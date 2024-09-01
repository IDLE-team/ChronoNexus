using UnityEngine;

public class MovableEntityStatePatrol : MovableEntityState
{
    private Vector3 _destination;
    private float _remainingDistance = 1f;
    private Transform[] _patrolPoints;
    private int _nextPointIndex = 0;
    private float _waitTime = 5f;
    private float _tempWaitTime;
    public MovableEntityStatePatrol(MovableEntity movableEntity, StateMachine stateMachine) : base(movableEntity,
        stateMachine)
    {
    }
    public override void Enter()
    {
        _tempWaitTime = _waitTime;
        _patrolPoints = _movableEntity.PatrolPoints;
        _destination = _patrolPoints[0].position;
        _navMeshAgent.SetDestination(_destination);
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        if (_tempWaitTime > 0)
        {
            _tempWaitTime -= Time.deltaTime;
        }
        base.LogicUpdate();
    }
    public override void PhysicsUpdate()
    {
        if (_navMeshAgent.remainingDistance <= _remainingDistance)
        {
            if (_tempWaitTime <= 0)
            {
                _tempWaitTime = _waitTime;
                if (_patrolPoints.Length == 1)
                {
                    //Осматривает вокруг
                }
                else
                {
                    _nextPointIndex++;
                    if (_nextPointIndex >= _patrolPoints.Length)
                    {
                        _nextPointIndex = 0;
                    }
                    _destination = _patrolPoints[_nextPointIndex].position;
                    _navMeshAgent.SetDestination(_destination);
                }
            }
        }
        base.PhysicsUpdate();
    }
}