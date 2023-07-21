using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _rangeWeapon;
    [SerializeField] private GameObject _bullet;

    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private VisualEffect _visualHitEffect;

    [SerializeField] private int _damage;

    private GameObject _bulletInstance;

    private Vector3 _shootDir;

    public void StartAttack()
    {
        _characterController.Animator.SetTrigger(_characterController.CharacterAnimation.animIDAttack);
    }

    public void Fire()
    {
        _characterController.Animator.SetTrigger(_characterController.CharacterAnimation.animIDShoot);
    }

    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackZone.transform.position, _attackZone.attackRadius, enemyLayer);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
        }
    }

    public void Shoot()
    {
        if (_characterController.CharacterTargetLock._nearestTarget != null)
        {
            _shootDir = (_characterController.CharacterTargetLock._nearestTarget.position - _rangeWeapon.transform.position).normalized;
        }
        else
        {
            _shootDir = transform.forward;
        }
        _bulletInstance = Instantiate(_bullet, _rangeWeapon.position, Quaternion.LookRotation(_shootDir));
        _bulletInstance.GetComponent<Bullet>().SetTarget(_shootDir);
    }

    public void PlayEffect()
    {
        _visualHitEffect.gameObject.SetActive(true);
        _visualHitEffect.Play();
        _characterController.AudioController.PlayHitSound();
    }
}