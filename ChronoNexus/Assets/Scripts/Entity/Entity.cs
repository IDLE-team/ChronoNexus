using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamagable, ITargetable, ITimeAffected, ISeeker
{
    public event Action OnTargetInvalid;
    public event Action OnTimeAffectedDestroy;
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    public Equiper Equiper => _equiper;
    public WeaponController WeaponController => _weaponController;

    public event Action OnSeekStart;
    public event Action OnSeekEnd;
    public bool IsTargetFound { get; set; }
    public ITargetable Target { get; set; }


    [SerializeField] protected TargetSelection _selection;
    [SerializeField] protected Transform _selfAimTargetTransform;
    public Transform SelfAim => _selfAimTargetTransform;
    [SerializeField] protected AudioClip _hitClip;
    [SerializeField] protected ParticleSystem _hitEffect;
    [SerializeField] protected DebugEnemySpawner _enemySpawner;
    [SerializeField] protected Equiper _equiper;
    [SerializeField] protected WeaponController _weaponController;
    public State state = new State();
    protected EnemyState _startState;

    protected AudioSource _audioSource;
    protected EnemyAnimator _animator;
    protected Rigidbody _rigidbody;

    protected EnemyLoot _loot;
    protected TargetFinder _targetFinder;

    public TargetFinder TargetFinder => _targetFinder;
    protected Health _health;
    protected StateMachine _stateMachine;

    protected bool _isAlive;
    protected bool _isValid = true;
    protected bool _isTarget;

    public static List<GameObject> enemyList = new List<GameObject>();
    public event Action OnDie;
    public event Action OnUIUpdate;

    public EntityStateDummy DummyState { get; private set; }

    public virtual string CurrentState
    {
        get
        {
            switch (_stateMachine.CurrentState)
            {
                case EnemyDummyState:
                    return State.Dummy.ToString();
                default:
                    return " ";
            }
        }
    }

    protected virtual void Awake()
    {
        InitializeParam();
    }

    protected virtual void InitializeStartState()
    {
        _stateMachine.Initialize(DummyState);
    }

    protected virtual void InitializeParam()
    {
        _stateMachine = new StateMachine();


        _targetFinder = GetComponent<TargetFinder>();
        _health = GetComponent<Health>();
        _animator = GetComponent<EnemyAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _loot = GetComponent<EnemyLoot>();
        _rigidbody = GetComponent<Rigidbody>();

        IsTargetFound = false;

        DummyState = new EntityStateDummy(this, _stateMachine);
    }

    protected void Start()
    {
        _isAlive = true;
        enemyList.Add(gameObject);
        InitializeStartState();
    }

    protected void Update()
    {
        if (!isTimeStopped)
        {
            _stateMachine.CurrentState.LogicUpdate();
        }
    }

    protected void FixedUpdate()
    {
        if (!isTimeStopped)
        {
            _stateMachine.CurrentState.PhysicsUpdate();
        }
    }

    public virtual void TakeDamage(float damage, bool isCritical)
    {
        if (!_isAlive)
            return;

        _health.Decrease(damage, isCritical);
        DamageEffect();
        _animator.PlayTakeDamageAnimation();
    }

    protected virtual void OnDied()
    {
        OnTargetInvalid?.Invoke();

        _isValid = false;
        _isAlive = false;

        SetSelfTarget(false);
        StopSeek();

        _stateMachine.ChangeState(DummyState);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        if (isTimeStopped)
        {
            RealTimeDieWaiter().Forget();
            return;
        }

        Die();
    }

    protected virtual void Die()
    {
        _animator.PlayDeathAnimation();

        if (_enemySpawner != null)
        {
            _enemySpawner.UpdateSliderValue();
        }

        OnDie?.Invoke();
    }

    public virtual void DieDestroy()
    {
        OnTimeAffectedDestroy?.Invoke();
        Destroy(gameObject, 1f);
    }


    public virtual void SetSelfTarget(bool _isActive)
    {
        _isTarget = _isActive;

        if (_isTarget)
            _selection.Select();
        else
            _selection.Deselect();
    }

    protected virtual void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    public void StartSeek()
    {
        OnSeekStart?.Invoke();
    }

    public void StopSeek()
    {
        OnSeekEnd?.Invoke();
    }

    protected void OnEnable()
    {
        _health.Died += OnDied;
        _stateMachine.OnStateChanged += UpdateUI;
    }

    protected void OnDisable()
    {
        StopSeek();
        _health.Died -= OnDied;
        _stateMachine.OnStateChanged -= UpdateUI;
    }

    protected void UpdateUI()
    {
        OnUIUpdate?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        OnTimeAffectedDestroy?.Invoke();
        enemyList.Remove(gameObject);
    }

    public virtual void RealTimeAction()
    {
        if (gameObject != null)
        {
            isTimeStopped = false;
            isTimeSlowed = false;

            _animator.ContinueAnimation();
        }
    }

    public virtual void StopTimeAction()
    {
        if (gameObject != null)
        {
            isTimeStopped = true;
            _animator.StopAnimation();
        }
    }

    public virtual void SlowTimeAction()
    {
        if (gameObject != null)
        {
            isTimeSlowed = true;

            _animator.SlowAnimation();
        }
    }

    public virtual void RewindTimeAction()
    {
        throw new NotImplementedException();
    }

    public virtual void AcceleratedTimeAction()
    {
        throw new NotImplementedException();
    }

    public Transform GetTransform()
    {
        return _selfAimTargetTransform;
    }

    public bool GetTargetSelected()
    {
        return _isTarget;
    }

    public bool GetTargetValid()
    {
        return _isValid;
    }

    protected virtual async UniTask RealTimeDieWaiter()
    {
        await UniTask.WaitUntil(() => !isTimeStopped);
        OnDied();
    }

    public virtual void TargetFoundReaction()
    {

    }

    public enum State
    {
        Dummy,
        Idle,
        Patrol,
        RandomMove,
        Chase,
        MeleeAttack,
        RangeAttack
    };
}