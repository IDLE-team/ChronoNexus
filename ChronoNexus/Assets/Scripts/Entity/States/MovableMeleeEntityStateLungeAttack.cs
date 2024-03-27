using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MovableMeleeEntityStateLungeAttack : MovableMeleeEntityState
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

    private bool _inSlash;

    private Quaternion toRotation;
    private CancellationTokenSource cancellationTokenSource;
    private Vector3 direction;
    private Tweener tween;

    public MovableMeleeEntityStateLungeAttack(MovableMeleeEntity movableEntity, StateMachine stateMachine) : base(
        movableEntity, stateMachine)
    {

    }

    public override void Enter()
    {
        _maxDistanceBetweenTarget = _movableMeleeEntity.MeleeAttacker.MaxMeleeAttackDistance;
        _minDistanceBetweenTarget = _movableMeleeEntity.MeleeAttacker.MinMeleeDistanceToTarget;
        _attackInterval = _movableMeleeEntity.MeleeAttacker.MeleeAttackInterval;

        _attackingAgentSpeed = _movableMeleeEntity.MeleeAttacker.MeleeAttackAgentSpeed;
        _minDelay = _movableMeleeEntity.MeleeAttacker.MinDelayTokenMelee;
        _maxDelay = _movableMeleeEntity.MeleeAttacker.MaxDelayTokenMelee;

        _targetPosition = _movableMeleeEntity.Target.GetTransform().position;

        _isAttack = true;
        _movableMeleeEntity.TargetFinder.SetWeight(1);
        _movableMeleeEntity.NavMeshAgent.SetDestination(_movableMeleeEntity.transform.position);
        //cancellationTokenSource = new CancellationTokenSource();
        //MeleeAttackAndRetreat(cancellationTokenSource.Token).Forget();

        base.Enter();
    }

    public override void Exit()
    {
        _isAttack = false;
        _movableMeleeEntity.NavMeshAgent.isStopped = false;
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
        toRotation =
                     Quaternion.LookRotation(
                         new Vector3(_targetPosition.x, _movableMeleeEntity.transform.position.y, _targetPosition.z) -
                         _movableMeleeEntity.transform.position, Vector3.up);

        if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) >
            _movableMeleeEntity.MeleeAttacker.MaxMeleeLungeDistance
            || Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) <
            _movableMeleeEntity.MeleeAttacker.MaxMeleeAttackDistance
           )
        {
            _stateMachine.ChangeState(_movableMeleeEntity.ChaseState);
            return;
        }


        

        if (_movableMeleeEntity.isTimeSlowed)
            _movableMeleeEntity.transform.rotation = Quaternion.Slerp(_movableMeleeEntity.transform.rotation,
                toRotation, 6f * 0.2f * Time.deltaTime);
        else
            _movableMeleeEntity.transform.rotation = Quaternion.Slerp(_movableMeleeEntity.transform.rotation,
                toRotation, 6f * Time.deltaTime);


        /*if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _retreatPosition) > 0.2f)
        {
            _movableMeleeEntity.StartMoveAnimation();
        }
        else
        {
            _movableMeleeEntity.EndMoveAnimation();
        }*/

        if (_movableMeleeEntity.MeleeAttacker.MeleeAttackTimer > 0)
        {
            if (_movableMeleeEntity.isTimeSlowed)
                _movableMeleeEntity.MeleeAttacker.DecreaseAttackTimer(Time.deltaTime * 0.2f);
            else
                _movableMeleeEntity.MeleeAttacker.DecreaseAttackTimer(Time.deltaTime);
        }

        if (_movableMeleeEntity.MeleeAttacker.MeleeAttackTimer <= 0 || !_inSlash)
        {
            _movableMeleeEntity.StartMoveAnimation();
            _movableMeleeEntity.MeleeAttacker.ResetAttackTimer();
            _movableMeleeEntity.StartAttackAnimation();
            //bottom anim
            direction = (_targetPosition - _movableMeleeEntity.SelfAim.transform.position);
            direction.y = 0;
            Vector3 newPosition = direction.normalized * _maxDistanceBetweenTarget * 2f;
            //newPosition.y = _movableMeleeEntity.transform.position.y;
            _inSlash = true;
            _movableMeleeEntity.transform.DOMove(_movableMeleeEntity.transform.position + newPosition, 1f).OnComplete(
                () =>
                {
                    _inSlash = false;
                    _movableMeleeEntity.EntityAnimator.PlayLurgeAnimation();
                });
            _navMeshAgent.SetDestination(_movableMeleeEntity.transform.position + newPosition);
            _movableMeleeEntity.transform.rotation = toRotation;
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
        _movableMeleeEntity.NavMeshAgent.speed = _movableMeleeEntity.MeleeAttacker.MeleeAttackAgentSpeed;
    }

    private async UniTask MeleeAttackAndRetreat(CancellationToken cancellationToken)
    {
        while (_isAttack && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay((int) _attackInterval * 1000);
            if (_movableMeleeEntity == null)
                cancellationTokenSource.Cancel();
            if (_movableMeleeEntity.Target == null)
                cancellationTokenSource.Cancel();
            if (Vector3.Distance(_movableMeleeEntity.SelfAim.transform.position, _targetPosition) >
                _maxDistanceBetweenTarget)
                cancellationTokenSource.Cancel();
            if (!cancellationToken.IsCancellationRequested)
            {

                await UniTask.Yield();
            }

        }

        await UniTask.Yield();
    }
}