using System;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

public class Attacker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _enemyLayer;

    [SerializeField]
    private Transform _rangeWeapon;

    [SerializeField]
    private Bullet _bullet;

    [SerializeField]
    private AttackZone _attackZone;

    [SerializeField]
    private VisualEffect _visualHitEffect;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private Character _character;

    [SerializeField]
    private InputService _inputService;

    private Vector3 _shootDir;

    [SerializeField]
    private CharacterAnimator _animator;

    [Inject]
    private void Construct(IInputService inputService, CharacterAnimator animator)
    {
        //_inputService = inputService;
        //_animator = animator;
    }

    private void OnEnable()
    {
        _inputService.Attacked += _animator.Attack;
        _inputService.Shot += _animator.Fire;
    }

    [UsedInAnimator]
    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(
            _attackZone.transform.position,
            _attackZone.Radius,
            _enemyLayer
        );
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
        }
    }

    [UsedInAnimator]
    public void Shoot()
    {
        if (_character.CharacterTargetingSystem.Target != null)
        {
            _shootDir = (
                _character.CharacterTargetingSystem.Target.position
                - _rangeWeapon.transform.position
            ).normalized;
        }
        else
        {
            _shootDir = transform.forward;
        }

        var bullet = Instantiate(
            _bullet,
            _rangeWeapon.position,
            Quaternion.LookRotation(_shootDir)
        );
        bullet.SetTarget(_shootDir);
    }

    [UsedInAnimator]
    public void PlayEffect()
    {
        _visualHitEffect.gameObject.SetActive(true);
        _visualHitEffect.Play();
        _character.AudioController.PlayHitSound();
    }

    private void OnDisable()
    {
        _inputService.Attacked -= _animator.Attack;
        _inputService.Shot -= _animator.Fire;
    }
}
