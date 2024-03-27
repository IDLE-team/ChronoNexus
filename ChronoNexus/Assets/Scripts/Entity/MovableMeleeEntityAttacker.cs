using UnityEngine;
using UnityEngine.VFX;

public class MovableMeleeEntityAttacker : Attacker
{
    [Header("Melee Attack")]
    //protected float _attackTimer;
    [SerializeField] private WeaponData _meleeWeaponData;

    [SerializeField] private AttackZone _attackZone;
    public AttackZone AttackZone => _attackZone;
    
    [SerializeField] private VisualEffect _visualHitEffect;
    [SerializeField] private float _meleeDamage = 10f;
    
    [SerializeField] protected float _meleeAttackInterval = 1f;

    [SerializeField] protected float _minDelayTokenMelee = 0.3f;
    [SerializeField] protected float _maxDelayTokenMelee = 1f;

    [SerializeField] protected float _maxMeleeAttackDistance = 1.5f;
    [SerializeField] protected float _minMeleeDistanceToTarget = 1f;

    [SerializeField] protected float _meleeAttackAgentSpeed = 4f;
    [SerializeField] protected float _defaultAgentSpeed = 1.5f;
    [SerializeField] protected float _maxMeleeLungeDistance = 4f;
     protected float _meleeAttackTimer = 0f;

     public float MeleeAttackTimer => _meleeAttackTimer;
    public float MaxMeleeLungeDistance => _maxMeleeLungeDistance;
    public float MeleeAttackInterval => _meleeAttackInterval;
    public float MinDelayTokenMelee => _minDelayTokenMelee;
    public float MaxDelayTokenMelee => _maxDelayTokenMelee;
    public float MinMeleeDistanceToTarget => _minMeleeDistanceToTarget;
    public float MaxMeleeAttackDistance => _maxMeleeAttackDistance;
    public float MeleeAttackAgentSpeed => _meleeAttackAgentSpeed;
    public float DefaultAgentSpeed => _defaultAgentSpeed;

    public WeaponData MeleeWeaponData => _meleeWeaponData;

    public void DecreaseAttackTimer(float value)
    {
        _meleeAttackTimer -= value;
    }

    public void ResetAttackTimer()
    {
        _meleeAttackTimer = _meleeAttackInterval;
        
    }
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
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_meleeDamage, false);
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