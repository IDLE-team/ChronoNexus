using UnityEngine;
using UnityEngine.VFX;

public class EnemyMeleeAttacker : Attacker
{
    [Header("Melee Attack Values")]
    //protected float _attackTimer;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private VisualEffect _visualHitEffect;
    [SerializeField] private float _meleeDamage;
    
    [SerializeField] protected float _meleeAttackInterval = 1f;

    [SerializeField] protected float _minDelayTokenMelee = 0.3f;
    [SerializeField] protected float _maxDelayTokenMelee = 1f;

    [SerializeField] protected float _maxMeleeAttackDistance = 2f;
    [SerializeField] protected float _minMeleeDistanceToTarget = 1f;

    [SerializeField] protected float _meleeAttackAgentSpeed = 4f;
    [SerializeField] protected float _defaultAgentSpeed = 1.5f;

    public float MeleeAttackInterval => _meleeAttackInterval;
    public float MinDelayTokenMelee => _minDelayTokenMelee;
    public float MaxDelayTokenMelee => _maxDelayTokenMelee;
    public float MinMeleeDistanceToTarget => _minMeleeDistanceToTarget;
    public float MaxMeleeAttackDistance => _maxMeleeAttackDistance;
    public float MeleeAttackAgentSpeed => _meleeAttackAgentSpeed;
    public float DefaultAgentSpeed => _defaultAgentSpeed;
    


    [UsedInAnimator]
    public void Hit()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(
            _attackZone.transform.position,
            _attackZone.Radius,
            _targetLayer
        );
        foreach (Collider collider in hitPlayer)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_meleeDamage);
        }
    }


    [UsedInAnimator]
    public void PlayEffect()
    {
        _visualHitEffect.gameObject.SetActive(true);
        _visualHitEffect.Play();
        //_enemy.AudioController.PlayHitSound();
    }
}