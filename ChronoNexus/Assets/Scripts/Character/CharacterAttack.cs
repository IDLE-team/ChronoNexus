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
    [SerializeField] private VisualEffect visualHitEffect;

    [SerializeField] private int damage;

    private GameObject _bulletInstance;
    private Vector3 _shootDir;

    public void StartAttack()
    {
        _characterController._animator.SetTrigger(_characterController._characterAnimation.animIDAttack);
    }

    public void Fire()
    {
        _characterController._animator.SetTrigger(_characterController._characterAnimation.animIDShoot);
    }

    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackZone.transform.position, _attackZone.attackRadius, enemyLayer);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage);
        }
    }

    public void Shoot()
    {
        if (_characterController._characterTargetLock._nearestTarget != null)
        {
            _shootDir = (_characterController._characterTargetLock._nearestTarget.position - _rangeWeapon.transform.position).normalized;
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
        visualHitEffect.gameObject.SetActive(true);
        visualHitEffect.Play();
        _characterController._audioController.PlayHitSound();
    }
}