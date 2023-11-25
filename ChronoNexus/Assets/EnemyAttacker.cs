using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using Zenject;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private Transform _rangeWeapon;

    [SerializeField]
    private Bullet _bulletPrefab;

    [SerializeField]
    private AttackZone _attackZone;

    [SerializeField]
    private VisualEffect _visualHitEffect;

    [SerializeField]
    private int _damage;

    [SerializeField]
    private Enemy _enemy;

    [SerializeField]
    private EnemyAnimator _animator;

    [Inject]
    private void Construct(IInputService inputService, CharacterAnimator animator)
    {
        //_inputService = inputService;
        //_animator = animator;
    }

    private void OnEnable()
    {
        // _inputService.Attacked += _animator.Attack;
        // _inputService.Shot += _animator.Fire;
    }

    [UsedInAnimator]
    public void Hit()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(
            _attackZone.transform.position,
            _attackZone.Radius,
            _playerLayer
        );
        foreach (Collider collider in hitPlayer)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
        }
    }

    [UsedInAnimator]
    public void Shoot(Vector3 target)
    {
        // if (_enemy.CharacterTargetingSystem.Target != null)
        // {
        //     _shootDir = (
        //         _enemy.CharacterTargetingSystem.Target.position
        //         - _rangeWeapon.transform.position
        //     ).normalized;
        // }
        // else
        // {
        //     _shootDir = transform.forward;
        // }

        // var bullet = Instantiate(
        //     _bullet,
        //     _rangeWeapon.position,
        //     Quaternion.LookRotation(transform)
        // );
        // bullet.SetTarget(transform);

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 spawnPosition = position + forward * 0.5f;
        Vector3 direction = (target - transform.position).normalized;
        spawnPosition.y = spawnPosition.y + 1.5f;
        var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
        bullet.SetTarget(direction);
    }

    [UsedInAnimator]
    public void PlayEffect()
    {
        _visualHitEffect.gameObject.SetActive(true);
        _visualHitEffect.Play();
        //_enemy.AudioController.PlayHitSound();
    }

    private void OnDisable()
    {
        // _inputService.Attacked -= _animator.Attack;
        // _inputService.Shot -= _animator.Fire;
    }
}
