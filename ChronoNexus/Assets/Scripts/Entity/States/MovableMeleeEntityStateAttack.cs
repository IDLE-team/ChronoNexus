using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MovableMeleeEntityStateAttack : MovableMeleeEntityState
{
    private Vector3 _targetPosition;
    private float _attackTimer = 0f;
    private float _attackInterval;
    private float _minDelay;
    private float _maxDelay;
    private float _maxDistanceBetweenTarget;
    private float _minDistanceBetweenTarget;
    private bool _isAttack = false;
    private float _attackingAgentSpeed;
    private Vector3 _randomDirection;
    private Vector3 _retreatPosition;

    private Quaternion toRotation;
    private CancellationTokenSource cancellationTokenSource;

    public MovableMeleeEntityStateAttack(MovableMeleeEntity movableEntity, StateMachine stateMachine) : base(
        movableEntity, stateMachine)
    {

    }

    public override void
        Enter() //если расстояние начинает увеличиваться, когда удар уже начат, враг начнет движение в сторону цели
    {
        _maxDistanceBetweenTarget = _movableMeleeEntity.MeleeAttacker.MaxMeleeAttackDistance;
        _minDistanceBetweenTarget = _movableMeleeEntity.MeleeAttacker.MinMeleeDistanceToTarget;
        _attackInterval = _movableMeleeEntity.MeleeAttacker.MeleeAttackInterval;
        _attackingAgentSpeed = _movableMeleeEntity.MeleeAttacker.MeleeAttackAgentSpeed;
        _minDelay = 0.1f;
        _maxDelay = 0.1f;

        _targetPosition = _movableMeleeEntity.Target.GetTransform().position;

        _isAttack = true;
        //_movableMeleeEntity.TargetFinder.SetWeight(1);

        cancellationTokenSource = new CancellationTokenSource();
        MeleeAttackAndRetreat(cancellationTokenSource.Token).Forget();
        _minDelay = _movableMeleeEntity.MeleeAttacker.MinDelayTokenMelee;
        _maxDelay = _movableMeleeEntity.MeleeAttacker.MaxDelayTokenMelee;
        /*_retreatPosition = _movableMeleeEntity.SelfAim.transform.position +
                           (_movableMeleeEntity.SelfAim.position - _targetPosition).normalized *
                           (_minDistanceBetweenTarget );
        _retreatPosition =
            new Vector3(_retreatPosition.x, _movableMeleeEntity.transform.position.y, _retreatPosition.z);

        _movableMeleeEntity.NavMeshAgent.SetDestination(_retreatPosition);*/


        base.Enter();
    }

    public override void Exit()
    {
        _isAttack = false;

        _movableMeleeEntity.IsTargetFound = false;
        //_movableMeleeEntity.TargetFinder.ResetTarget();
        //_movableMeleeEntity.NavMeshAgent.speed = 1.5f;
        // _movableMeleeEntity.EndMoveAnimation();

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

        
        toRotation =
            Quaternion.LookRotation(
                new Vector3(_targetPosition.x, _movableMeleeEntity.transform.position.y, _targetPosition.z) -
                _movableMeleeEntity.transform.position, Vector3.up);

        if (_movableMeleeEntity.isTimeSlowed)
            _movableMeleeEntity.transform.rotation = Quaternion.Slerp(_movableMeleeEntity.transform.rotation,
                toRotation, 6f * 0.2f * Time.deltaTime);
        else
            _movableMeleeEntity.transform.rotation = Quaternion.Slerp(_movableMeleeEntity.transform.rotation,
                toRotation, 6f * Time.deltaTime);


        if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) >
            _maxDistanceBetweenTarget)
        {
            _stateMachine.ChangeState(_movableMeleeEntity.ChaseState);
            return;
        }

        if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _retreatPosition) > 0.2f)
        {
            //_movableMeleeEntity.EntityAnimator.SetMoveAnimation(true);
        }
        else
        {
            //_movableMeleeEntity.EntityAnimator.SetMoveAnimation(false);
        }

        if (_movableMeleeEntity.MeleeAttacker.MeleeAttackTimer > 0)
        {
            if (_movableMeleeEntity.isTimeSlowed)
                _movableMeleeEntity.MeleeAttacker.DecreaseAttackTimer(Time.deltaTime * 0.2f);
            else
                _movableMeleeEntity.MeleeAttacker.DecreaseAttackTimer(Time.deltaTime);
        }
        else
        {
            _movableMeleeEntity.StartAttackAnimation();
            _movableMeleeEntity.MeleeAttacker.ResetAttackTimer();
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

        _targetPosition = _movableMeleeEntity.Target.GetTransform().position;
        base.PhysicsUpdate();
    }

    protected override async UniTask TimeWaiter()
    {
        await UniTask.WaitUntil(() => !_movableMeleeEntity.isTimeSlowed && !_movableMeleeEntity.isTimeStopped);
        //_movableMeleeEntity.NavMeshAgent.speed = _movableMeleeEntity.MeleeAttacker.MeleeAttackAgentSpeed;
    }

    private async UniTask MeleeAttackAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int) (Random.Range(_minDelay, _maxDelay) * 1000));
            if (_movableMeleeEntity == null)
                cancellationTokenSource.Cancel();
            if (_movableMeleeEntity.Target == null)
                cancellationTokenSource.Cancel();
            if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) >
                _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();
            if (!cancellationToken.IsCancellationRequested
                && Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) <=
                _maxDistanceBetweenTarget
                && Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) >
                _minDistanceBetweenTarget)
            {
                do
                {
                    _randomDirection = Random.insideUnitSphere.normalized;
                    _retreatPosition = _movableMeleeEntity.SelfAim.transform.position + _randomDirection *
                        ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    _retreatPosition = new Vector3(_retreatPosition.x, _movableMeleeEntity.SelfAim.position.y,
                        _retreatPosition.z);
                } while (Vector3.Distance(_targetPosition, _retreatPosition) < _minDistanceBetweenTarget
                         || Vector3.Distance(_targetPosition, _retreatPosition) > _maxDistanceBetweenTarget);

                if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _retreatPosition) > 0.2f)
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(_retreatPosition);
                    //_movableMeleeEntity.NavMeshAgent.speed = _attackingAgentSpeed;
                }
                else
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.SelfAim.transform.position);
                }

                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested
                     && Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) <=
                     _minDistanceBetweenTarget)
            {
                do
                {
                    _randomDirection = Random.insideUnitSphere.normalized;
                    _retreatPosition = _movableMeleeEntity.SelfAim.transform.position + _randomDirection *
                        ((_minDistanceBetweenTarget + _maxDistanceBetweenTarget) / 2);
                    _retreatPosition = new Vector3(_retreatPosition.x, _movableMeleeEntity.SelfAim.position.y,
                        _retreatPosition.z);
                } while (Vector3.Distance(_targetPosition, _retreatPosition) <= _minDistanceBetweenTarget);

                if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _retreatPosition) > 0.2f)
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(_retreatPosition);
                    //_movableMeleeEntity.NavMeshAgent.speed = _attackingAgentSpeed;
                }
                else
                {
                    _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.SelfAim.transform.position);
                }

                await UniTask.Yield();
            }
            else if (!cancellationToken.IsCancellationRequested)
            {
                _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.SelfAim.transform.position);
                await UniTask.Yield();
            }

        }

        await UniTask.Yield();
    }
}