using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterMovement), typeof(CharacterAnimator))]
[RequireComponent(typeof(CharacterAudioController), typeof(Health))]

public class Character : MonoBehaviour, IDamagable, ITargetable
{
    [SerializeField] private Transform _aimTarget;

    [SerializeField] private Slider _hpBar;
    

    private IOutfitter _outfitter;

    public Transform AimTarget => _aimTarget;
        
    [SerializeField]  private Health _health;
    public Health Health => _health;
    public Rigidbody Rigidbody { get; private set; }
    public CharacterAudioController AudioController { get; private set; }
    public CharacterMovement Movement { get; private set; }
    public CharacterAnimator Animator { get; private set; }
    
    [SerializeField] private СharacterTargetingSystem _characterTargetingSystem;
    public СharacterTargetingSystem CharacterTargetingSystem => _characterTargetingSystem;

    [SerializeField] private CharacterEventsHolder _сharacterEventsHolder;
    public CharacterEventsHolder CharacterEventsHolder => _сharacterEventsHolder;

    [SerializeField] private Equiper _equiper;
    public Equiper Equiper => _equiper;
    
    [SerializeField] private WeaponController _weaponController;
    public WeaponController WeaponController => _weaponController;


    public AimRigController AimRigController { get; private set; }
    private PlayerAttacker Attacker { get; set; }


    private bool _isValid = true;

    private InventoryItemManager _inventoryItemManager;

    public InventoryItemManager InventoryItemManager => _inventoryItemManager;

    public Transform Transform => transform;

    private bool _isInvincible;

    [Inject]
    private void Construct(InventoryItemManager itemManager)
    {
        _inventoryItemManager = itemManager;
    }


    private void OnEnable()
    {
        _health.Died += Die;
    }

    private void OnDisable()
    {
        _health.Died -= Die;
    }

    private void Awake()
    {
        //TODO прокинуть через Zenject
        _health = GetComponent<Health>();
        Movement = GetComponent<CharacterMovement>();
        Attacker = GetComponent<PlayerAttacker>();
        _characterTargetingSystem = GetComponent<СharacterTargetingSystem>();
        Animator = GetComponent<CharacterAnimator>();
        Rigidbody = GetComponent<Rigidbody>();
        AudioController = GetComponent<CharacterAudioController>();
        AimRigController = GetComponent<AimRigController>();
        _equiper = GetComponent<Equiper>();
        _weaponController = GetComponent<WeaponController>();
    }
    private void Start()
    {
        _inventoryItemManager.OnEquiped += Equiper.EquipWeapon;
        Equiper.EquipWeapon(_inventoryItemManager.GetEquipedGun());

    }

    public void TakeDamage(float damage, bool isCritical)
    {
        if (!_isInvincible)
            _health.Decrease(damage, isCritical);
    }

    public void Die()
    {
        _isValid = false;
        OnTargetInvalid?.Invoke();
        Death().Forget();
    }

    public IEnumerator SetInvincibleWithDuration(float duration)
    {
        SetInvincible(true);
        yield return new WaitForSeconds(duration);
        SetInvincible(false);

    }
    private async UniTaskVoid Death()
    {
        Destroy(gameObject);
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        GameController.Instance.Death();
        await UniTask.Yield();
    }

    public void SetSelfTarget(bool isActive)
    {
        throw new NotImplementedException();
    }

    public Transform GetTransform()
    {
        return _aimTarget;
    }

    public GameObject GetTargetGameObject()
    {
        return gameObject;
    }

    public bool GetTargetSelected()
    {
        return _isValid;
    }

    public void SetInvincible(bool isInvincible)
    {
        _isInvincible = isInvincible;
    }

    public bool GetTargetValid()
    {
        return _isValid;
    }

    public event Action OnTargetInvalid;

}