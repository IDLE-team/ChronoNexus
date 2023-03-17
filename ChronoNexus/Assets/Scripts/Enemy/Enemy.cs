using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    #region States

    public StateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyPatrolState PatrolState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }

    #endregion States

    #region FieldOfView

    [Range(0, 360)] public float viewAngle;
    public float viewRadius;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    #endregion FieldOfView

    public NavMeshAgent navMeshAgent;
    public Transform player;

    public float Health { get; private set; }
    public bool canSeePlayer = false;

    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private GameObject _targetIndicator;

    private AudioSource _audioSource;
    private Animator _animator;
    private Transform _target;

    private static string Player => "Player";

    private bool _isAlive;
    private bool _isTarget;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        StateMachine = new StateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        PatrolState = new EnemyPatrolState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(PatrolState);

        StartCoroutine(nameof(FindTargetsWithDelay), .2f);

        _isAlive = true;
    }

    private void Update()
    {
        Debug.Log(StateMachine.CurrentState);
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void TakeDamage(int damage)
    {
        if (_isAlive)
        {
            Health -= damage;
            DamageEffect();
            _animator.SetTrigger("TakeHit");

            if (Health < 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        Health = 0;
        _isAlive = false;
        _animator.SetBool("Dead", true);
        Destroy(gameObject, 0.8f);
    }

    public void ToggleSelfTarget()
    {
        _isTarget = !_isTarget;
        _targetIndicator.SetActive(_isTarget);
    }

    private void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    #region FieldOfView

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
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            _target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, _target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (_target.CompareTag(Player))
                    {
                        player = _target;
                        canSeePlayer = true;
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

    #endregion FieldOfView
}