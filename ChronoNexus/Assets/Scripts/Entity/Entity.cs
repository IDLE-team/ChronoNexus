using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public abstract class Entity : MonoBehaviour, IDamagable, IFinisherable, ITargetable, ITimeAffected, ISeeker
{
    public Attacker _Attacker;
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
    [SerializeField] protected ParticleSystem _finisherReadyVFX;
    [SerializeField] protected float _finisherHPTreshold;
    public State state = new State();
    protected EnemyState _startState;

    protected AudioSource _audioSource;
    protected EntityAnimator _animator;
    public EntityAnimator EntityAnimator => _animator;
    protected Rigidbody _rigidbody;
    [SerializeField]protected Collider _collider;
    protected EnemyLoot _loot;
    protected TargetFinder _targetFinder;

    public TargetFinder TargetFinder => _targetFinder;
    protected Health _health;
    protected StateMachine _stateMachine;

    protected bool _isAlive;
    protected bool _isValid = true;
    protected bool _isTarget;

    protected bool _isFinisherReady;
    
    
    
    public static List<GameObject> enemyList = new List<GameObject>();
    public event Action OnDie;
    public event Action OnUIUpdate;
    public event Action OnFinisherReady;
    public event Action OnFinisherEnded;
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
        InitializeIndividualParam();
        
        enemyList.Add(gameObject);

        _stateMachine = new StateMachine();
        _targetFinder = GetComponent<TargetFinder>();
        _health = GetComponent<Health>();
        _animator = GetComponent<EntityAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _loot = GetComponent<EnemyLoot>();
        _rigidbody = GetComponent<Rigidbody>();
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
        

        IsTargetFound = false;

        DummyState = new EntityStateDummy(this, _stateMachine);
        
    }

    protected virtual void InitializeIndividualParam()
    {
        
    }

    public virtual void RotateTo(Transform target)
    {
        float _duration = 1f;
        if (isTimeStopped)
        {
            return;
        }

        if (isTimeSlowed)
        {
            _duration *= 8;
        }
        transform.DOLookAt(new Vector3(target.position.x,transform.position.y,target.position.z),_duration);
    }

    protected void Start()
    {
        _isAlive = true;
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

        if (_health.Value <= _finisherHPTreshold && !_isFinisherReady && _isAlive)
        {
            _isFinisherReady = true;
            OnFinisherReady?.Invoke();
            _finisherReadyVFX.Play();
        }
    }

    protected virtual void Die()
    {
        OnTargetInvalid?.Invoke();
        OnFinisherEnded?.Invoke();
        OnTimeAffectedDestroy?.Invoke();

        _isFinisherReady = false;
        _isValid = false;
        _isAlive = false;
        
        _finisherReadyVFX.Stop();

        SetSelfTarget(false);
        StopSeek();

        _stateMachine.ChangeState(DummyState);

        _collider.enabled = false;
        
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        if (isTimeStopped)
        {
            RealTimeDieWaiter().Forget();
            return;
        }
        
        if(!_animator.GetAnimationParamStatus("Finisher"))
            _animator.PlayDeathAnimation();

        if (_enemySpawner != null)
        {
            _enemySpawner.UpdateSliderValue();
        }

        OnDie?.Invoke();
        enemyList.Remove(gameObject);
        Destroy(gameObject, 3f);
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
        _health.Died += Die;
        _stateMachine.OnStateChanged += UpdateUI;
    }

    protected void OnDisable()
    {
        StopSeek();
        _health.Died -= Die;
        _stateMachine.OnStateChanged -= UpdateUI;
    }

    protected void UpdateUI()
    {
        OnUIUpdate?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        OnTimeAffectedDestroy?.Invoke();
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
        Die();
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

    public virtual void ResetValues()
    {
        
    }
    
    public void StartFinisher()
    {
       _animator.Finisher();
       _stateMachine.ChangeState(DummyState);
       ResetValues();
       var Player = GameObject.FindWithTag("Player");
       Vector3 dir = Player.transform.position - transform.position;
       dir.y = 0;
       transform.rotation = Quaternion.LookRotation(-dir);
       _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
       _rigidbody.velocity = Vector3.zero;
    }

    public bool GetFinisherableStatus()
    {
        if (!_isAlive)
            return false;
        return _isFinisherReady;
    }
    
}