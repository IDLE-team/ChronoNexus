using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MovableEntity : Entity
{
    [Header("Idle parameters")] [SerializeField]
    protected bool _isCanTurn;

    [Header("Patrol parameters")] [SerializeField]
    protected bool _isPatrol;

    [SerializeField] protected float _patrolSpeed = 1.5f;

    public float PatrolSpeed => _patrolSpeed;

    [SerializeField] protected Vector3[] _patrolPoints;

    public Vector3[] PatrolPoints => _patrolPoints;

    [Header("Random Move parameters")] [SerializeField]
    protected float _randomMoveMaxDistance;

    public float RandomMoveMaxDistance => _randomMoveMaxDistance;

    [SerializeField] protected float _randomMoveSpeed = 1.5f;

    public float RandomMoveSpeed => _randomMoveSpeed;

    [Header("Chase parameters")] public NavMeshAgent NavMeshAgent => _navMeshAgent;

    protected NavMeshAgent _navMeshAgent;

    protected float _lastNavAcceleration;
    protected float _lastNavSpeed;
    protected float _lastNavAngularSpeed;

    public MovableEntityStateIdle IdleState { get; private set; }
    public MovableEntityStateRandomMove RandomMoveState { get; private set; }
    public MovableEntityStateChase ChaseState { get; private set; }
    public MovableEntityStatePatrol PatrolState { get; private set; }

    protected override void InitializeStartState()
    {

        switch (state)
        {
            case State.Dummy:
                _stateMachine.Initialize(DummyState);
                break;
            case State.Idle:
                _stateMachine.Initialize(IdleState);
                break;
            case State.RandomMove:
                _stateMachine.Initialize(RandomMoveState);
                break;
            case State.Patrol:
                _stateMachine.Initialize(PatrolState);
                break;
            case State.Chase:
                _stateMachine.Initialize(ChaseState);
                break;
            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
    }

    protected override void InitializeParam()
    {
        base.InitializeParam();

        _navMeshAgent = GetComponent<NavMeshAgent>();

        RandomMoveState = new MovableEntityStateRandomMove(this, _stateMachine);
        PatrolState = new MovableEntityStatePatrol(this, _stateMachine);
        IdleState = new MovableEntityStateIdle(this, _stateMachine);
        ChaseState = new MovableEntityStateChase(this, _stateMachine);

    }

    protected override void InitializeIndividualParam()
    {
    }

    protected override void OnDied()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.speed = 0;
        _navMeshAgent.angularSpeed = 0;

        _navMeshAgent.isStopped = true;
        base.OnDied();
    }

    protected override void Die()
    {
        _navMeshAgent.velocity = Vector3.zero;
        _loot.DropLoot();

        base.Die();
    }

    public override void RealTimeAction()
    {
        base.RealTimeAction();
        if (gameObject != null)
            if (_isAlive)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _lastNavSpeed;
                _navMeshAgent.angularSpeed = _lastNavAngularSpeed;
            }
    }

    public override void StopTimeAction()
    {

        if (gameObject != null)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.speed = 0;
            _navMeshAgent.angularSpeed = 0;
            _navMeshAgent.velocity = Vector3.zero;
        }

        base.StopTimeAction();
    }

    public override void SlowTimeAction()
    {

        if (gameObject != null)
        {
            SetLastNavMeshValues();

            _navMeshAgent.speed = 0.1f;
            _navMeshAgent.angularSpeed = 0.1f;
        }

        base.SlowTimeAction();
    }

    public void StartMoveAnimation()
    {
        _animator.StartMoveAnimation();
    }

    public void EndMoveAnimation()
    {
        _animator.EndMoveAnimation();
    }

    private void SetLastNavMeshValues()
    {
        _lastNavSpeed = _navMeshAgent.speed;
        _lastNavAngularSpeed = _navMeshAgent.angularSpeed;
    }

    public override void TargetFoundReaction()
    {
        _stateMachine.ChangeState(ChaseState);
    }

    public virtual void AgentDestinationSet()
    {
        if (Vector3.Distance(SelfAim.transform.position, Target.GetTransform().position) <= 1.5f)
        {
            _navMeshAgent.SetDestination(SelfAim.transform.position);
            EndMoveAnimation();
        }
        else
        {
            _navMeshAgent.SetDestination(Target.GetTransform().position);
            StartMoveAnimation();
        }
    }

    public virtual void TargetChaseDistanceSwitch()
    {
        if (Vector3.Distance(SelfAim.position, Target.GetTransform().position) >
            12f) //view distance or check last point
            _stateMachine.ChangeState(RandomMoveState);
    }
}