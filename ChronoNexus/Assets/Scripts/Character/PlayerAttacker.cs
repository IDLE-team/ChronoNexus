using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Zenject;

public class PlayerAttacker : Attacker
{
    [SerializeField] private Transform _rangeWeapon;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private VisualEffect _visualHitEffect;
    [SerializeField] private Character _character;
    [SerializeField] private InputService _inputService;
    [SerializeField] private WeaponController _weaponController;
    
    [SerializeField] private float _damage;

    [SerializeField] private WeaponData _weaponData;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
 //   [SerializeField] private GameObject _virtualCamera;

    
    private PlayerInputActions _input;
    private Vector3 _shootDir;
    
    
    public float Damage => _damage;
    private bool _canFire = true;
    private bool _resetTimer;

    [Inject]
    private void Construct(PlayerInputActions input/*, CharacterAnimator animator*/)
    {
        _input = input;
        _input.Player.Fire.performed += OnFire;
        _input.Player.Finisher.performed += OnFinisher;

     //   _animator = animator;
    }
    

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        StartFire();
    }
    private void OnFinisher(InputAction.CallbackContext obj)
    {
        StartFinisherAttack();
    }

    public void StartFire()
    {
        if (_weaponController.CurrentWeapon == null)
            return;
        if (_character.CharacterTargetingSystem.Target == null)
        {
            _character.AimRigController.SetSmoothWeight(1);

            if (_canFire)
            {
                StartCoroutine(TimerToReset());
            }
            else
            {
                _resetTimer = true;
            }
        }
        _character.Animator.Fire(Animator.StringToHash(_weaponController.CurrentWeapon.WeaponAnimation.ToString()));
    }

    public void StartFinisherAttack()
    {
        if (_weaponController.CurrentWeapon == null)
            return;
        _character.Equiper.EquipWeapon(_weaponData);

        _character.Animator.Finisher();
        

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            _weaponController.CurrentWeapon.StopFire();
        }
    }

    IEnumerator TimerToReset()
    {
        _canFire = false;
        yield return new WaitForSeconds(1f);
        if (_character.CharacterTargetingSystem.Target != null)
        {
            _canFire = true;
            _resetTimer = false;
            yield return null;
        }
        
        if (_resetTimer)
        {
            StartCoroutine(TimerToReset());
            _resetTimer = false;
            _character.AimRigController.StopSmoothWeight();
            yield return null;
        }

 
        if (_weaponController.CurrentWeapon.isFire)
        {
            StartCoroutine(TimerToReset());
            _resetTimer = false;
            _character.AimRigController.StopSmoothWeight();
            yield return null;
        }
        
        else if(!_resetTimer)
        {
            _canFire = true;
            _resetTimer = false;
            if (_character.CharacterTargetingSystem.Target != null)
            {
                _canFire = true;
                _resetTimer = false;
                yield return null;
            }

            else
            {
                _character.AimRigController.SetSmoothWeight(0);
            }
        }
    }
    
    
    [UsedInAnimator]
    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackZone.transform.position, _attackZone.Radius, _targetLayer);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage, false);
        }
    }
    

    [UsedInAnimator]
    public void PlayEffect()
    {
        _visualHitEffect.gameObject.SetActive(true);
        _visualHitEffect.Play();
        _character.AudioController.PlayHitSound();
    }

}