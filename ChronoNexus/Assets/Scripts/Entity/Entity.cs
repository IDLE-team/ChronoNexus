using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

public abstract class Entity : MonoBehaviour, IDamagable, IFinisherable, ITargetable, ITimeAffected, ISeeker
{
    [SerializeField] private BuffLoot _buffLoot;
    [SerializeField] private SkinnedMeshRenderer _renderer;
    public event Action OnTargetInvalid;
    public event Action OnTimeAffectedDestroy;
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    private bool _isFinisherKill;
    public Equiper Equiper => _equiper;
    public WeaponController WeaponController => _weaponController;

    public event Action OnSeekStart;
    public event Action OnSeekEnd;
    public bool IsTargetFound { get; set; }
    public ITargetable Target { get; set; }
    protected EnemyAnimationEventsHolder _entityAnimationEventsHolder;
    [SerializeField] protected TargetSelection _selection;
    [SerializeField] protected Transform _selfAimTargetTransform;
    public Transform SelfAim => _selfAimTargetTransform;
    [SerializeField] protected AudioClip _hitClip;
    [SerializeField] protected ParticleSystem _hitEffect;
    [SerializeField] protected DebugEnemySpawner _enemySpawner;
    protected Equiper _equiper;
    [SerializeField] protected WeaponController _weaponController;
    [SerializeField] protected GameObject _finisherReady;
    [SerializeField] protected float _finisherHPTreshold;
    
    
    
    
    
    public State state = new State();
    protected EntityState _startState;

    protected AudioSource _audioSource;
    protected EntityAnimator _animator;
    public EntityAnimator EntityAnimator => _animator;
    protected Rigidbody _rigidbody;
    [SerializeField] protected Collider _collider;
    protected EntityLoot _loot;
    protected TargetFinder _targetFinder;
    protected bool _isRotating = false;
    public bool IsRotating => _isRotating;
    public TargetFinder TargetFinder => _targetFinder;
    protected Health _health;
    protected StateMachine _stateMachine;

    public bool IsAlive => _isAlive;

    protected bool _isAlive;

    protected bool _isValid = true;

    protected bool _isTarget;

    protected bool _isFinisherReady;


    public static List<GameObject> enemyList = new List<GameObject>();


    public event Action OnDie;
    public event Action OnUIUpdate;
    public event Action OnFinisherReady;
    public event Action OnFinisherEnded;
    public event Action OnFinisherInvalid;


    public EntityStateDummy DummyState { get; private set; }

    public virtual IState CurrentState
    {
        get { return _stateMachine.CurrentState; }
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
        _loot = GetComponent<EntityLoot>();
        _rigidbody = GetComponent<Rigidbody>();
        _equiper = GetComponent<Equiper>();
        
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }

        
        _targetFinder.OnTargetFinded += TargetFoundReaction;
        
        
        
        

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

        if (_isRotating)
        {
            return;
            
        }

        _isRotating = true;
        transform.DOLookAt(new Vector3(target.position.x, transform.position.y, target.position.z), _duration)
            .OnComplete(
                () => { _isRotating = false; });
    }

    protected void Start()
    {
        _isAlive = true;
        InitializeStartState();
        StartSeek();
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
        if (Target == null && CurrentState != DummyState && !_isRotating)
        {
            transform.DORotateQuaternion(Quaternion.Euler(new Vector3(0, 1, 0) * 180f) * transform.rotation, 1f);
        }

        _health.Decrease(damage, isCritical);
        DamageEffect();
        _animator.PlayTakeDamageAnimation();

        if (_health.Value <= _finisherHPTreshold && !_isFinisherReady && _isAlive)
        {
            _isFinisherReady = true;
            OnFinisherReady?.Invoke();
            _finisherReady.SetActive(true);
        }
    }

    protected virtual void Die()
    {
        if(!_isAlive)
            return;
        
        if (_isFinisherKill)
            _buffLoot.DropBuff();
        
        OnTargetInvalid?.Invoke();
        OnFinisherEnded?.Invoke();
        OnTimeAffectedDestroy?.Invoke();
        OnFinisherInvalid?.Invoke();
        
        
        
        _isFinisherReady = false;
        _isValid = false;
        _isAlive = false;

        _finisherReady.SetActive(false);

        SetSelfTarget(false);
        StopSeek();

        _stateMachine.ChangeState(DummyState);

        _collider.enabled = false;

        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        if (isTimeStopped)
        {
            RealTimeDieWaiter().Forget();
            return;
        }

        if (!_animator.GetAnimationParamStatus("Finisher"))
            _animator.PlayDeathAnimation();

        if (_enemySpawner != null)
        {
            _enemySpawner.UpdateSliderValue();
        }

        _loot.DropItems();
        OnDie?.Invoke();
        enemyList.Remove(gameObject);
        StartCoroutine(StartDissolveWithDelay());
        WeaponController.ClearWeapon();
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

    protected virtual void OnEnable()
    {
        _health.Died += Die;
        _stateMachine.OnStateChanged += UpdateUI;
    }

    protected virtual void OnDisable()
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
            if (Target == null)
            {
                StartSeek();
            }
            isTimeStopped = false;
            isTimeSlowed = false;

            _animator.ContinueAnimation();
        }
    }

    public virtual void StopTimeAction()
    {
        if (gameObject != null)
        {
            StopSeek();
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

    public GameObject GetTargetGameObject()
    {
        return gameObject;
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

    public virtual void TargetFoundReaction(ITargetable target)
    {
        Target = target;
        IsTargetFound = true;
        StopSeek();

    }
    public virtual void TargetLossReaction()
    {
        Target = null;
        IsTargetFound = false;
        StartSeek();
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

    public virtual void StartFinisher(int id)
    {
        _isFinisherKill = true;
        _animator.Finisher(id);
        _stateMachine.ChangeState(DummyState);
        _finisherReady.SetActive(false);
        ResetValues();
        var Player = GameObject.FindWithTag("Player");
        Vector3 dir = Player.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(-dir);
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _rigidbody.linearVelocity = Vector3.zero;
    }

    public void Dissolve()
    {
        if(_renderer)
            _renderer.material.DOFloat(1, "_DissolveAmount", 1f);
    }

    IEnumerator StartDissolveWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Dissolve();
    }
    public bool GetFinisherableStatus()
    {
        if (!_isAlive)
            return false;
        return _isFinisherReady;
    }

}