using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttack : PlayerController
{
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private VisualEffect visualHitEffect;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private int damage;

    public void StartAttack()
    {
        _animator.SetTrigger(_playerAnimation.animIDAttack);
    }

    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackZone.transform.position, _attackZone.attackRadius, enemyLayer);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(damage);
        }
    }

    public void PlayEffect()
    {
        visualHitEffect.gameObject.SetActive(true);
        visualHitEffect.Play();
        _audioController.PlayHitSound();
    }
}