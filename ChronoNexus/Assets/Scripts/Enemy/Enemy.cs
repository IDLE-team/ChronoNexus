using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAnimator), typeof(AudioSource), typeof(Health))]
[RequireComponent(typeof(TargetFinder), typeof(TimeBody), typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAttacker), typeof(EnemyLoot))]
public class Enemy : MonoBehaviour, IDamagable, ITargetable, ISeeker, ITimeAffected
{
    [SerializeField]
    private TargetSelection _selection;

    [SerializeField]
    private Transform _aimTargetTransform;

    [SerializeField]
    private AudioClip _hitClip;

    [SerializeField]
    private ParticleSystem _hitEffect;

    [SerializeField]
    private Bullet _bulletPrefab;

    [SerializeField]
    private DebugEnemySpawner enemySpawner;

    [SerializeField]
    private bool _isTarget;

    private EnemyLoot _loot;

    private bool _isValid = true;


    private bool _isDamagable = true;
    public bool isDamagable => _isDamagable;
    private bool _isLowHPBuffSelected;


    public EnemyType enemyType = new EnemyType();

    public State state = new State();



    public static List<GameObject> enemyList = new List<GameObject>();

    public event Action OnSeekStart;

    public event Action OnSeekEnd;

    public event Action OnUIUpdate;

    public event Action OnTimeAffectedDestroy;

    public event Action OnDie;

    public event Action OnTargetInvalid;

    public string CurrentState
    {
        get
        {
            switch (_stateMachine.CurrentState)
            {
                case EnemyDummyState:
                    return State.Dummy.ToString();
                case EnemyIdleState:
                    return State.Idle.ToString();
                case EnemyPatrolState:
                    return State.Patrol.ToString();
                case EnemyChaseState:
                    return State.Chase.ToString();
                case EnemyRangeAttackState:
                    return State.Attack.ToString();
                case EnemyMeleeAttackState:
                    return State.Attack.ToString();
                default:
                    return " ";
            }
            ;
        }
    }
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public EnemyAttacker EnemyAttacker => _enemyAttacker;
    public TargetFinder TargetFinder => _targetFinder;

    private StateMachine _stateMachine;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyDummyState DummyState { get; private set; }

    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }

    public EnemyMeleeAttackState MeleeAttackState { get; private set; }
    public EnemyRangeAttackState RangeAttackState { get; private set; }
    public JuggernautRangeAttackState JuggernautAttackState { get; private set; }

    public EnemyFearState FearState { get; private set; }
    public EnemyAggressionState AggressionState { get; private set; }
    public EnemyPrudenceState PrudenceState { get; private set; }

    public Transform Target { get; set; }
    public bool IsTargetFound { get; set; }
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    private Health _health;
    private NavMeshAgent _navMeshAgent;
    private AudioSource _audioSource;
    private EnemyAnimator _animator;
    private EnemyState _startState;
    private EnemyAttacker _enemyAttacker;
    private TargetFinder _targetFinder;

    private bool _isAlive;

    private void Awake()
    {
        _targetFinder = GetComponent<TargetFinder>();   
        _health = GetComponent<Health>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _enemyAttacker = GetComponent<EnemyAttacker>();
        _loot = GetComponent<EnemyLoot>();

        _stateMachine = new StateMachine();
        IdleState = new EnemyIdleState(this, _stateMachine);
        DummyState = new EnemyDummyState(this, _stateMachine);

        PatrolState = new EnemyPatrolState(this, _stateMachine);
        ChaseState = new EnemyChaseState(this, _stateMachine);

        RangeAttackState = new EnemyRangeAttackState(this, _stateMachine);
        MeleeAttackState = new EnemyMeleeAttackState(this, _stateMachine);
        JuggernautAttackState = new JuggernautRangeAttackState(this, _stateMachine);

        FearState = new EnemyFearState(this, _stateMachine);
        AggressionState = new EnemyAggressionState(this, _stateMachine);
        PrudenceState = new EnemyPrudenceState(this, _stateMachine);

        //  _stateMachine.Initialize(DummyState);

        IsTargetFound = false;
    }

    private void Start()
    {
        _isAlive = true;
        _isLowHPBuffSelected = false;
        enemyList.Add(gameObject);
        //Debug.Log(state);
        if (enemyType == Enemy.EnemyType.Juggernaut)
        {
            _isDamagable = false;
        }
        switch (state)
        {
            case State.Dummy:
                _stateMachine.Initialize(DummyState);
                break;

            case State.Idle:
                _stateMachine.Initialize(IdleState);
                break;

            case State.Patrol:
                _stateMachine.Initialize(PatrolState);
                break;

            case State.Chase:
                _stateMachine.Initialize(ChaseState);
                break;

            case State.Attack:
                switch (enemyType)
                {
                    case Enemy.EnemyType.Guard:
                        _stateMachine.ChangeState(MeleeAttackState);
                        break;
                    case Enemy.EnemyType.Stormtrooper:
                        _stateMachine.ChangeState(RangeAttackState);
                        break;
                    default:
                        _stateMachine.ChangeState(RangeAttackState);
                        break;
                }
                break;

            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
        //Debug.Log(enemyList.Count);
    }

    public void InitializeSpawner(DebugEnemySpawner spawner, EnemyState startState)
    {
        enemySpawner = spawner;
        //_stateMachine.ChangeState(startState);
    }

    private void Update()
    {
        if (!isTimeStopped)
        {
            _stateMachine.CurrentState.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!isTimeStopped)
        {
            _stateMachine.CurrentState.PhysicsUpdate();
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_isAlive)
            return;

        
        if(_stateMachine.CurrentState != DummyState && Target == null)
        {
            _navMeshAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        }
        
        if (_enemyAttacker.Immortality)
        {
            return;
        }
        if (!_isLowHPBuffSelected && _health.Value <= _health.MaxHealth / 4 && _stateMachine.CurrentState != DummyState)
        {
            _isLowHPBuffSelected = true;
            //изменить на настраиваемую
            Debug.Log("Сделать настройку шанса для каждого стейта");
            float _chance = UnityEngine.Random.Range(1, 4);
            switch (_chance)
            {
                case 1:
                case 2:
                    // flags for states
                    //_stateMachine.ChangeState(AggressionState);
                    Debug.Log("AggressionState");
                    break;
                case 3:
                    //_stateMachine.ChangeState(FearState);
                    Debug.Log("FearState");
                    break;
                case 4:
                    //_stateMachine.ChangeState(PrudenceState);
                    Debug.Log("PrudenceState");
                    break;
            }
        }
        _health.Decrease(damage);
        DamageEffect();
        _animator.PlayTakeDamageAnimation();

    }

    private void OnDied()
    {
        OnTargetInvalid?.Invoke();
        _isValid = false;
        _isAlive = false;
        _animator.PlayDeathAnimation();
        OnTimeAffectedDestroy?.Invoke();
        SetSelfTarget(false);
        StopSeek();
        _loot.DropLoot();
        if (enemySpawner != null)
        {
            enemySpawner.UpdateSliderValue();
        }
        OnDie?.Invoke();
        Destroy(gameObject, 1f);
    }

    public void SetSelfTarget(bool _isActive)
    {
        _isTarget = _isActive;

        if (_isTarget)
            _selection.Select();
        else
            _selection.Deselect();
    }

    private void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    public void Shoot(Vector3 target)
    {
        _enemyAttacker.Shoot(target);
    }

    public void MeleeAttack()
    {
        //_enemyAttacker.Hit();
        // hit привязан к аниматору(((
    }

    public void StartMoveAnimation()
    {
        _animator.StartMoveAnimation();
    }
    public void SetMoveX(int value)
    {
        _animator.SetMoveX(value);
    }

    public void EndMoveAnimation()
    {
        _animator.EndMoveAnimation();
    }

    public void StartAttackAnimation()
    {
        _animator.PlayAttackAnimation();
    }

    public void StartSeek()
    {
        OnSeekStart?.Invoke();
    }

    public void StopSeek()
    {
        OnSeekEnd?.Invoke();
    }

    private void OnEnable()
    {
        _health.Died += OnDied;
        _stateMachine.OnStateChanged += UpdateUI;
    }

    private void OnDisable()
    {
        StopSeek();
        _health.Died -= OnDied;
        _stateMachine.OnStateChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        OnUIUpdate?.Invoke();
    }

    private void OnDestroy()
    {
        OnTimeAffectedDestroy?.Invoke();
        enemyList.Remove(gameObject);
        // enemyList.Remove(gameObject);
        /*if (enemySpawner != null)

            enemySpawner.DestroyEnemy(gameObject);
        */
    }

    public void RealTimeAction()
    {
        if (gameObject != null)
        {
            isTimeStopped = false;
            isTimeSlowed = false;

            _navMeshAgent.isStopped = false;

            _animator.ContinueAnimation();
        }
    }

    public void StopTimeAction()
    {
        if (gameObject != null)
        {
            isTimeStopped = true;
            _navMeshAgent.isStopped = true;
            _animator.StopAnimation();
        }
    }

    public void SlowTimeAction()
    {
        if (gameObject != null)
        {
            isTimeSlowed = true;
            _navMeshAgent.speed = 0.1f;
            _navMeshAgent.acceleration = 0.1f;

            _animator.SlowAnimation();
        }
    }

    public void RewindTimeAction()
    {
        throw new NotImplementedException();
    }

    public void AcceleratedTimeAction()
    {
        throw new NotImplementedException();
    }

    public Transform GetTransform()
    {
        return _aimTargetTransform;
    }
    public bool GetTargetValid()
    {
        return _isValid;
    }
    public enum State
    {
        Dummy,
        Idle,
        Patrol,
        Chase,
        Attack,
        Fear,
        Aggression,
        Prudence
    };

    public enum EnemyType
    {
        Stormtrooper,
        Guard,
        Juggernaut
    };
    /*public enum EnemyPattern
    {
        Fear,
        Aggression,
        Prudence
    };*/
}
