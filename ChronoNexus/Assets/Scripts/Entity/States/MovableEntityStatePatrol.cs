using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class MovableEntityStatePatrol : MovableEntityState
{
    private Vector3 _destination;
    private float _remainingDistance = 1f;
    private Vector3[] _patrolPoints;
    private int _nextPointIndex = 0;

    public MovableEntityStatePatrol(MovableEntity movableEntity, StateMachine stateMachine) : base(movableEntity,
        stateMachine)
    {
        //_remainingDistance = _entity.EntityLogic.RemainingDistanceToRandomPosisiton
    }

    public override void Enter()
    {
        _movableEntity.StartSeek();
        //_movableEntity.EntityAnimator.SetMoveAnimation(true);

        _destination = _patrolPoints[0];

        _navMeshAgent.SetDestination(_destination);

        base.Enter();
    }

    public override void Exit()
    {
       // _movableEntity.EntityAnimator.SetMoveAnimation(false);
        _movableEntity.StopSeek();

        base.Exit();
    }

    public override void LogicUpdate()
    {
        CheckTarget();
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        if (_navMeshAgent.remainingDistance <= _remainingDistance)
        {
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

                _destination = _patrolPoints[_nextPointIndex];
                _navMeshAgent.SetDestination(_destination);
            }
        }

        base.PhysicsUpdate();
    }
}