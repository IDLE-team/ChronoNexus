using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    #region States

    public StateMachine enemySM;
    public EnemyIdleState IdleState;
    public EnemyPatrolState PatrolState;
    public EnemyChaseState ChaseState;

    #endregion States

    #region FieldOfView

    public float viewRadius;

    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    #endregion FieldOfView

    [SerializeField] private AudioClip _hitClip;
    public float health { get; private set; }

    private ParticleSystem _hitEffect;
    private AudioSource _audioSource;
    private Animator _animator;
    public NavMeshAgent _navMeshAgent;
    public Transform target;
    public Transform player;

    public bool canSeePlayer = false;

    private bool _isAlive;
    private bool _targeted;

    private void Start()
    {
        enemySM = new StateMachine();

        IdleState = new EnemyIdleState(this, enemySM);
        PatrolState = new EnemyPatrolState(this, enemySM);
        ChaseState = new EnemyChaseState(this, enemySM);

        enemySM.Initialize(PatrolState);

        StartCoroutine("FindTargetsWithDelay", .2f);

        _hitEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _isAlive = true;
    }

    private void Update()
    {
        Debug.Log(enemySM.CurrentState);
        enemySM.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        enemySM.CurrentState.PhysicsUpdate();
    }

    public void TakeDamage(int damage)
    {
        if (_isAlive)
        {
            health -= damage;
            DamageEffect();
            _animator.SetTrigger("TakeHit");

            if (health < 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        health = 0;
        _isAlive = false;
        _animator.SetBool("Dead", true);
        Destroy(gameObject, 0.8f);
    }

    private void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    public void ToggleSelfTarget()
    {
        _targeted = !_targeted;
        transform.GetChild(0).gameObject.SetActive(_targeted);
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
            target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (target.CompareTag("Player"))
                    {
                        player = target;
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