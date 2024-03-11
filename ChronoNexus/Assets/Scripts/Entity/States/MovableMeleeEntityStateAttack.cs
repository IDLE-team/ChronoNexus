using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MovableMeleeEntityStateAttack : MovableMeleeEntityState
{
    private Vector3 _targetPosition;
    private float attackTimer = 0f;
    private float attackInterval = 1f;
    private float minDelay = 1f;
    private float maxDelay = 2f;
    private float _maxDistanceBetweenTarget = 1.5f;
    private float _minDistanceBetweenTarget = 1f;
    private bool _isAttack = false;
    private float _defaultAgentSpeed = 1.5f;
    private float _attackingAgentSpeed = 2.5f;
    private Vector3 randomDirection;
    private Vector3 retreatPosition;

    //Vector3 targetPosition;
    private Quaternion toRotation;
    private CancellationTokenSource cancellationTokenSource;

    public MovableMeleeEntityStateAttack(MovableMeleeEntity movableEntity, StateMachine stateMachine) : base(movableEntity, stateMachine)
    {
        //_remainingDistance = _entity.EntityLogic.RemainingDistanceToRandomPosisiton
    }

    public override void Enter()
    {
        //attackInterval = _movableMeleeEntity.EnemyAttacker.MeleeAttackInterval;
        _targetPosition = _movableMeleeEntity.Target.position;
        _isAttack = true;
        _movableMeleeEntity.TargetFinder.SetWeight(1);
        cancellationTokenSource = new CancellationTokenSource();
        MeleeAttackAndRetreat(cancellationTokenSource.Token).Forget();
        base.Enter();
    }

    public override void Exit()
    {
        _isAttack = false;
        _movableMeleeEntity.IsTargetFound = false;
        _movableMeleeEntity.NavMeshAgent.speed = 1.5f;
        _movableMeleeEntity.EndMoveAnimation();
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (_movableMeleeEntity.Target == null)
        {
            cancellationTokenSource.Cancel();
            _stateMachine.ChangeState(_movableMeleeEntity.RandomMoveState);
            return;
        }

        toRotation = Quaternion.LookRotation(_targetPosition - _movableMeleeEntity.transform.position, Vector3.up);
        if (_movableMeleeEntity.isTimeSlowed)
            _movableMeleeEntity.transform.rotation = Quaternion.Slerp(_movableMeleeEntity.transform.rotation, toRotation, 6f * 0.2f * Time.deltaTime);
        else
            _movableMeleeEntity.transform.rotation = Quaternion.Slerp(_movableMeleeEntity.transform.rotation, toRotation, 6f * Time.deltaTime);
        if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_movableMeleeEntity.ChaseState);
            return;
        }
        if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, retreatPosition) > 0.2f)
        {
            _movableMeleeEntity.StartMoveAnimation();
        }
        else
        {
            _movableMeleeEntity.EndMoveAnimation();
        }

        if (attackTimer > 0)
        {
            if (_movableMeleeEntity.isTimeSlowed)
                attackTimer -= Time.deltaTime * 0.2f;
            else
                attackTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0)
        {
            _movableMeleeEntity.StartAttackAnimation();
            _movableMeleeEntity.MeleeAttack();
            attackTimer = attackInterval;
        }

        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        if (_movableMeleeEntity.Target == null)
        {
            cancellationTokenSource.Cancel();
            return;
        }

        _targetPosition = _movableMeleeEntity.Target.position;
        base.PhysicsUpdate();
    }

    private async UniTask MeleeAttackAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int) (Random.Range(minDelay, maxDelay) * 1000));
            if (_movableMeleeEntity == null) 
                cancellationTokenSource.Cancel();
            if (_movableMeleeEntity.Target == null) 
                cancellationTokenSource.Cancel();
            if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) > _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();
            if (!cancellationToken.IsCancellationRequested 
                && Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) <= _maxDistanceBetweenTarget 
                && Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) > _minDistanceBetweenTarget)
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _movableMeleeEntity.SelfAim.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _movableMeleeEntity.SelfAim.transform.position.y, retreatPosition.z);
                } while (Vector3.Distance(_targetPosition, retreatPosition) < _minDistanceBetweenTarget 
                         || Vector3.Distance(_targetPosition, retreatPosition) > _maxDistanceBetweenTarget);

                if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, retreatPosition) > 0.2f)
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(retreatPosition);
                    _movableMeleeEntity.NavMeshAgent.speed = _attackingAgentSpeed;
                    _movableMeleeEntity.StartMoveAnimation();
                }
                else
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.SelfAim.transform.position);
                    _movableMeleeEntity.EndMoveAnimation();
                }
                //_movableMeleeEntity.TargetFinder.SetWeight(1);
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested 
                     && Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) <= _minDistanceBetweenTarget)
            {
                do
                {
                    randomDirection = Random.insideUnitSphere.normalized;
                    retreatPosition = _movableMeleeEntity.SelfAim.transform.position + randomDirection * ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    retreatPosition = new Vector3(retreatPosition.x, _movableMeleeEntity.SelfAim.transform.position.y, retreatPosition.z);
                } while (Vector3.Distance(_targetPosition, retreatPosition) <= _minDistanceBetweenTarget);

                if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, retreatPosition) > 0.2f)
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(retreatPosition);
                    _movableMeleeEntity.NavMeshAgent.speed = _attackingAgentSpeed;
                    _movableMeleeEntity.StartMoveAnimation();
                }
                else
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.SelfAim.transform.position);
                    _movableMeleeEntity.EndMoveAnimation();
                }
                //_movableMeleeEntity.TargetFinder.SetWeight(0);
                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested)
            {
                _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.SelfAim.transform.position);
                _movableMeleeEntity.NavMeshAgent.speed = _defaultAgentSpeed;
                _movableMeleeEntity.EndMoveAnimation();
                //_movableMeleeEntity.TargetFinder.SetWeight(0);
                await UniTask.Yield();
            }
            
        }
        await UniTask.Yield();
    }
}