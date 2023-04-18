using System;
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

    [SerializeField] private bool _isTarget;

    public event Action OnSeekStart;

    public event Action OnSeekEnd;

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
    private DebugEnemySpawner enemySpawner;

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

        _stateMachine.Initialize(DummyState);

        IsTargetFound = false;
    }

    private void Start()
    {
        _isAlive = true;
    }

    public void InitializeSpawner(DebugEnemySpawner spawner, EnemyState startState)
    {
        enemySpawner = spawner;
        _stateMachine.ChangeState(startState);
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
        _animator.TakeDamage();
    }

    private void OnDied()
    {
        _isAlive = false;
        _animator.Death();
        StopSeek();
        Destroy(gameObject, 0.8f);
    }

    public void ToggleSelfTarget()
    {
        _isTarget = !_isTarget;
        if (_isTarget)
            _selection.Select();
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
        Vector3 spawnPosition = position + forward * 1;
        Vector3 direction = (target - transform.position).normalized;
        //direction.y = direction.y + 0.1f;
        var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
        bullet.SetTarget(direction);
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
    }

    private void OnDisable()
    {
        StopSeek();
        _health.Died -= OnDied;
    }

    private void OnDestroy()
    {
        if (enemySpawner != null)
            enemySpawner.DestroyEnemy(gameObject);
    }
}