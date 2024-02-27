using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Zenject;

public class Attacker : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _rangeWeapon;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private VisualEffect _visualHitEffect;
    [SerializeField] private float _damage;
    [SerializeField] private Character _character;
    [SerializeField] private InputService _inputService;
    [SerializeField] private WeaponController _weaponController;
    public float Damage => _damage;
    private Vector3 _shootDir;
    private CharacterAnimator _animator;
    private bool _canFire = true;
    private bool _resetTimer;
    private PlayerInputActions _input;

    [Inject]
    private void Construct(PlayerInputActions input, CharacterAnimator animator)
    {
        _input = input;
        _input.Player.Fire.performed += OnFire;
        _input.Player.Hit.performed += OnHit;

        _animator = animator;


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
    private void OnHit(InputAction.CallbackContext obj)
    {
        _animator.Attack();
    }

    public void StartFire()
    {
        if (_weaponController.CurrentWeapon == null)
            return;
        if (_character.CharacterTargetingSystem.Target == null)
        {
            _character.AimRigController.SetWeight(1);
            _character.AimRigController.StopSmoothWeight();

            if (_canFire)
            {
                StartCoroutine(TimerToReset());
            }
            else
            {
                _resetTimer = true;
            }
        }
        
        _animator.Fire(Animator.StringToHash(_weaponController.CurrentWeapon.WeaponAnimation.ToString()));
        
    }

    IEnumerator TimerToReset()
    {
        _canFire = false;
        yield return new WaitForSeconds(1f);
        Debug.Log("ResetTimerStatus" + _resetTimer);
        if (_resetTimer)
        {
            StartCoroutine(TimerToReset());
            _resetTimer = false;
            _character.AimRigController.StopSmoothWeight();
            yield return null;
        }

        else
        {
            Debug.Log("Called");
            _canFire = true;
            _resetTimer = false;
            _character.AimRigController.SetSmoothWeight(0);
        }
    }
    
    
    [UsedInAnimator]
    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackZone.transform.position, _attackZone.Radius, _enemyLayer);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
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