using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAnimator), typeof(AudioSource))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IDamagable, ITargetable, ISeeker
{
    [SerializeField] private Selection _selection;
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private DebugEnemySpawner enemySpawner;

    [SerializeField] private bool _isTarget;


    public State state = new State();

    public static List<GameObject> enemyList = new List<GameObject>();

    public event Action OnSeekStart;

    public event Action OnSeekEnd;

    public event Action OnUIUpdate;

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
                default:
                    return " ";
            };


        }
    }
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private StateMachine _stateMachine;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyDummyState DummyState { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyRangeAttackState RangeAttackState { get; private set; }
    public Transform Target { get; set; }
    public bool IsTargetFound { get; set; }

    private IHealth _health;
    private NavMeshAgent _navMeshAgent;
    private AudioSource _audioSource;
    private EnemyAnimator _animator;
    private EnemyState _startState;


    private bool _isAlive;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimator>();
        _audioSource = GetComponent<AudioSource>();

        _stateMachine = new StateMachine();
        IdleState = new EnemyIdleState(this, _stateMachine);
        DummyState = new EnemyDummyState(this, _stateMachine);

        PatrolState = new EnemyPatrolState(this, _stateMachine);
        ChaseState = new EnemyChaseState(this, _stateMachine);
        RangeAttackState = new EnemyRangeAttackState(this, _stateMachine);

        //  _stateMachine.Initialize(DummyState);

        IsTargetFound = false;
    }

    private void Start()
    {
        _isAlive = true;
        enemyList.Add(gameObject);
        //Debug.Log(state);
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
                _stateMachine.Initialize(RangeAttackState);
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
        //   Debug.Log(_stateMachine.CurrentState);
        _stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }

    public void TakeDamage(float damage)
    {
        if (!_isAlive)
            return;
        _health.Decrease(damage);
        DamageEffect();
        _animator.PlayTakeDamageAnimation();
    }

    private void OnDied()
    {
        _isAlive = false;
        _animator.PlayDeathAnimation();
        StopSeek();
        if (enemySpawner != null)
        {
            enemySpawner.UpdateSliderValue();
        }
        Destroy(gameObject, 1f);
    }

    public void SetSelfTarget(bool _isActive)
    {
        _isTarget = _isActive;

        if (_isTarget) _selection.Select();

        else _selection.Deselect();
    }

    private void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    public void Shoot(Vector3 target)
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 spawnPosition = position + forward * 0.5f;
        Vector3 direction = (target - transform.position).normalized;
        spawnPosition.y = spawnPosition.y + 1.5f;
        var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
        bullet.SetTarget(direction);
    }

    public void StartMoveAnimation()
    {
        _animator.StartMoveAnimation();
    }

    public void EndMoveAnimation()
    {
        _animator.EndMoveAnimation();
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
        enemyList.Remove(gameObject);
        // enemyList.Remove(gameObject);
        /*if (enemySpawner != null)

            enemySpawner.DestroyEnemy(gameObject);
        */
    }

    public enum State
    {
        Dummy,
        Idle,
        Patrol,
        Chase,
        Attack
    };
}