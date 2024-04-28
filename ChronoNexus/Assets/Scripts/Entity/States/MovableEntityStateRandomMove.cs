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
    private float _distance;
    
    public MovableEntityStateRandomMove(MovableEntity movableEntity, StateMachine stateMachine):base(movableEntity, stateMachine)
    {
        //_remainingDistance = _entity.EntityLogic.RemainingDistanceToRandomPosisiton
    }
    public override void Enter()
    {
        _distance = _movableEntity.RandomMoveMaxDistance;
            
        _movableEntity.StartSeek();
        //_movableEntity.EntityAnimator.SetMoveAnimation(true);
        
        _destination = GetRandomDirection();
        
        _navMeshAgent.SetDestination(_destination);
        
        base.Enter();
    }

    public override void Exit()
    {
        //_movableEntity.EntityAnimator.SetMoveAnimation(false);
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
    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableEntity.isTimeSlowed && !_movableEntity.isTimeStopped);
        //_movableEntity.NavMeshAgent.speed = _movableEntity.RandomMoveSpeed;
        //Default speed
    }
    
    private Vector3 GetRandomDirection()
    {
        return _movableEntity.transform.position + new Vector3(Random.Range(-_distance, _distance), _movableEntity.SelfAim.transform.position.y, Random.Range(-_distance, _distance));
    }
}