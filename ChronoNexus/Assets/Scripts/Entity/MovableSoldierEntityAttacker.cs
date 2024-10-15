using UnityEngine;
using UnityEngine.VFX;

public class MovableEntitySoldierAttacker : MovableMeleeEntityAttacker
{
    [Header("Range Attack")] [SerializeField]
    private WeaponData _rangeWeaponData;

    [SerializeField] private float _rangeAttackInterval = 1f;

    [SerializeField] private float _minDelayTokenRange = 0.5f;
    [SerializeField] private float _maxDelayTokenRange = 2f;

    [SerializeField] private float _minRangeDistanceToTarget = 5f;
    [SerializeField] private float _maxRangeAttackDistance = 8f;

    [SerializeField] private float _rangeAttackAgentSpeed = 2f;
    
    [SerializeField] private float _preShootTime = 0;

    public float RangedAttackInterval => _rangeAttackInterval;
    public float MinDelayTokenRange => _minDelayTokenRange;
    public float MaxDelayTokenRange => _maxDelayTokenRange;
    public float MinRangeDistanceToTarget => _minRangeDistanceToTarget;
    public float MaxRangeAttackDistance => _maxRangeAttackDistance;
    public float RangeAttackAgentSpeed => _rangeAttackAgentSpeed;
    public float PreShootTime => _preShootTime;
    public WeaponData RangeWeaponData => _rangeWeaponData;
}