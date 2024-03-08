using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class MovableEntityStateChase : MovableEntityState
{
    private float _remainingDistance = 1f;

    public MovableEntityStateChase(MovableEntity movableEntity, StateMachine stateMachine) : base(movableEntity,
        stateMachine)
    {
        //_remainingDistance = _entity.EntityLogic.RemainingDistanceToRandomPosisiton
    }

    public override void Enter()
    {
        _movableEntity.StopSeek();
        _movableEntity.StartMoveAnimation();

        base.Enter();
    }

    public override void Exit()
    {
        _movableEntity.IsTargetFound = false;
    }

    public override void LogicUpdate()
    {
        TargetChaseDistanceSwitch();

        base.LogicUpdate();
    }


    public override void PhysicsUpdate()
    {
        if (_movableEntity.Target == null)
        {
            _stateMachine.ChangeState(_movableEntity.DummyState);
            return;
        }

        AgentDestinationSet();

        base.PhysicsUpdate();
    }

    protected virtual void AgentDestinationSet()
    {
        if (Vector3.Distance(_movableEntity.SelfAim.transform.position, _movableEntity.Target.position) <= 2f)
        {
            _navMeshAgent.SetDestination(_movableEntity.SelfAim.transform.position);
            _movableEntity.EndMoveAnimation();
        }
        else
        {
            _navMeshAgent.SetDestination(_movableEntity.Target.position);
            _movableEntity.StartMoveAnimation();
        }
    }

    protected virtual void TargetChaseDistanceSwitch()
    {
        _movableEntity.TargetChaseDistanceSwitch();
    }

    protected override void TargetFoundReaction()
    {
        //empty
    }
}