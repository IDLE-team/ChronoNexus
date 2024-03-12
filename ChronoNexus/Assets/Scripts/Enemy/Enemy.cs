using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

/*[RequireComponent(typeof(EnemyAnimator), typeof(AudioSource), typeof(Health))]
[RequireComponent(typeof(Rigidbody), typeof(Animator))]*/
public class Enemy : MonoBehaviour, IDamagable, ITargetable, ITimeAffected, ISeeker
{
    [SerializeField] protected TargetSelection _selection;

    [SerializeField] protected Transform _selfAimTargetTransform;
    public Transform SelfAim => _selfAimTargetTransform;

    [SerializeField] protected AudioClip _hitClip;

    [SerializeField] protected ParticleSystem _hitEffect;

    [SerializeField] protected DebugEnemySpawner _enemySpawner;
    [HideInInspector] public EnemyType enemyType = new EnemyType();
    public State state = new State();

    public ITargetable Target { get; set; }
    public bool IsTargetFound { get; set; }
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }


    protected TargetFinder _targetFinder;
    protected Health _health;
    protected AudioSource _audioSource;
    protected EnemyAnimator _animator;
    protected EnemyState _startState;
    protected Rigidbody _rigidbody;


    protected StateMachine _stateMachine;


    protected bool _isAlive;
    protected bool _isValid = true;
    protected bool _isTarget;


    public static List<GameObject> enemyList = new List<GameObject>();

    public event Action OnSeekStart;

    public event Action OnSeekEnd;

    public event Action OnUIUpdate;

    public event Action OnTimeAffectedDestroy;

    public event Action OnDie;

    public event Action OnTargetInvalid;


    
    public EnemyDummyState DummyState { get; protected set; }

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

    protected void Awake()
    {
        InitializeParam();
    }

    protected virtual void InitializeParam()
    {
        _health = GetComponent<Health>();
        _animator = GetComponent<EnemyAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();

        _stateMachine = new StateMachine();
        
        DummyState = new EnemyDummyState(this, _stateMachine);


        IsTargetFound = false;
    }

    protected void Start()
    {
        _isAlive = true;
        enemyList.Add(gameObject);
        InitializeStartState();
    }

    protected virtual void InitializeStartState()
    {
        switch (state)
        {
            case State.Dummy:
                _stateMachine.Initialize(DummyState);
                break;
            default:
                _stateMachine.Initialize(DummyState);
                break;
        }
    }


    public void InitializeSpawner(DebugEnemySpawner spawner, EnemyState startState)
    {
        _enemySpawner = spawner;
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

    protected void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    public void StartSeek()
    {
        OnSeekStart?.Invoke();
    }

    public  void StopSeek()
    {
        OnSeekEnd?.Invoke();
    }

    protected  void OnEnable()
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

    public void RewindTimeAction()
    {
        throw new NotImplementedException();
    }

    public void AcceleratedTimeAction()
    {
        throw new NotImplementedException();
    }

    public  Transform GetTransform()
    {
        return _selfAimTargetTransform;
    }

    public bool GetTargetSelected()
    {
        throw new NotImplementedException();
    }

    public  bool GetTargetValid()
    {
        return _isValid;
    }

    protected async UniTask RealTimeDieWaiter()
    {
        await UniTask.WaitUntil(() => !isTimeStopped);
        OnDied();
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
}