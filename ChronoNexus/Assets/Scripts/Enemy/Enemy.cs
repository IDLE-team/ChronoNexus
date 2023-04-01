using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAnimator), typeof(AudioSource))]
public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private Selection _selection;
    [SerializeField] private LayerMask _obstacleMask;
    
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private ParticleSystem _hitEffect;

    [SerializeField] [Range(0, 360)] private float _viewAngle;
    [SerializeField] [Min(0)] private float _viewRadius;
    private NavMeshAgent _navMeshAgent;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public float ViewAngle => _viewAngle;
    public float ViewRadius => _viewRadius;

    public float Health { get; private set; }
    
    private StateMachine _stateMachine;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    

    public Transform player;

    public bool canSeeTarget = false;


    private AudioSource _audioSource;
    private EnemyAnimator _animator;
    private Transform _target;

    private static string Player => "Player";

    private bool _isAlive;
    private bool _isTarget;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<EnemyAnimator>();
        _audioSource = GetComponent<AudioSource>();

        _stateMachine = new StateMachine();
        IdleState = new EnemyIdleState(this, _stateMachine);
        PatrolState = new EnemyPatrolState(this, _stateMachine);
        ChaseState = new EnemyChaseState(this, _stateMachine);
    }

    private void Start()
    {
        _stateMachine.Initialize(PatrolState);
        StartCoroutine(nameof(FindTargetsWithDelay), 0.2f);
        _isAlive = true;
    }

    private void Update()
    {
        //Debug.Log(_stateMachine.CurrentState);
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
        Health -= damage;
        DamageEffect();
        _animator.TakeDamage();
        if (Health < 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Health = 0;
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
    
    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, _targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            _target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, _target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                {
                    if (_target.CompareTag(Player))
                    {
                        player = _target;
                        canSeeTarget = true;
                    }
                }
            }
        }
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
}