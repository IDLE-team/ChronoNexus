using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public  class MovableEntityStateRandomMove : MovableEntityState
{
    private Vector3 _destination;
    private float _remainingDistance = 1f;
    
    public MovableEntityStateRandomMove(MovableEntity movableEntity, StateMachine stateMachine):base(movableEntity, stateMachine)
    {
        //_remainingDistance = _entity.EntityLogic.RemainingDistanceToRandomPosisiton
    }
    public override void Enter()
    {
        _movableEntity.StartSeek();
        _movableEntity.StartMoveAnimation();
        
        _destination = GetRandomDirection();
        
        _navMeshAgent.SetDestination(_destination);
        
        base.Enter();
    }

    public override void Exit()
    {
        _movableEntity.EndMoveAnimation();
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
            _destination = GetRandomDirection();
            _navMeshAgent.SetDestination(_destination);
        }
        base.PhysicsUpdate();
    }
    
    private Vector3 GetRandomDirection()
    {
        return _movableEntity.transform.position + new Vector3(Random.Range(-10f, 10f), _movableEntity.SelfAim.transform.position.y, Random.Range(-10f, 10f));
    }
}