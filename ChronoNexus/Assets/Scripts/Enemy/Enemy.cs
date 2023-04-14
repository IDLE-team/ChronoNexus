using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAnimator), typeof(AudioSource))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private Selection _selection;
    [SerializeField] private LayerMask _obstacleMask;

    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private ParticleSystem _hitEffect;

    [SerializeField][Range(0, 360)] private float _viewAngle;
    [SerializeField][Min(0)] private float _viewRadius;

    [SerializeField] private Bullet _bulletPrefab;

    public DebugEnemySpawner enemySpawner;

    private NavMeshAgent _navMeshAgent;
    private IHealth _health;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public float ViewAngle => _viewAngle;
    public float ViewRadius => _viewRadius;

    private StateMachine _stateMachine;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyDummyState DummyState { get; private set; }

    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }

    public EnemyRangeAttackState RangeAttackState { get; private set; }

    public Transform player;

    public bool canSeeTarget = false;

    private AudioSource _audioSource;
    private EnemyAnimator _animator;
    private Transform _target;

    private EnemyState _startState;

    private static string Player => "Player";

    private bool _isAlive;
    [SerializeField] private bool _isTarget;

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
    }

    private void OnEnable()
    {
        _health.Died += OnDied;
    }

    private void Start()
    {
        StartCoroutine(nameof(FindTargetsWithDelay), 0.2f);
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
        Vector3 spawnPosition = position + forward * 1.2f;
        Vector3 direction = (target - transform.position).normalized;
        var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
        bullet.SetTarget(direction);
    }

    [Fix("Заменить на UniTask/UniRx")]
    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    [Fix("Делигировать задачу поиска другому скрипту")]
    private void FindVisibleTargets()
    {
        var results = new Collider[30];
        var size = Physics.OverlapSphereNonAlloc(transform.position, ViewRadius, results, _targetMask);
        for (var i = 0; i < size; i++)
        {
            _target = results[i].transform;
            var dirToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) >= ViewAngle / 2)
                continue;

            var dstToTarget = Vector3.Distance(transform.position, _target.position);
            if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                continue;

            if (!_target.CompareTag(Player))
                continue;
            player = _target.GetComponent<Character>().AimTarget;
            canSeeTarget = true;
        }
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
    }

    private void OnDestroy()
    {
        if (enemySpawner != null)
            enemySpawner.DestroyEnemy(gameObject);
    }

#if UNITY_EDITOR

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

#endif
}