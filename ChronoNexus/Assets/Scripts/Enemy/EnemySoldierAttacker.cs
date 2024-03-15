using UnityEngine;
using UnityEngine.VFX;

public class EnemySoldierAttacker : EnemyMeleeAttacker
{

    [Header("Range Attack")]
    [SerializeField] private WeaponData _rangeWeaponData;

    [SerializeField] private float _rangeAttackInterval = 1f;

    [SerializeField] private float _minDelayTokenRange = 0.3f;
    [SerializeField] private float _maxDelayTokenRange = 1f;

    [SerializeField] private float _minRangeDistanceToTarget = 2f;
    [SerializeField] private float _maxRangeAttackDistance = 1f;

    [SerializeField] private float _rangeAttackAgentSpeed = 2f;
    
    [SerializeField] private int _ammoCount = 8;

    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private Bullet _prefabBullet;

    private Bullet _bullet;
    private Vector3 _bulletDirection;
    
    
    public float RangedAttackInterval => _rangeAttackInterval;
    public float MinDelayTokenRange => _minDelayTokenRange;
    public float MaxDelayTokenRange => _maxDelayTokenRange;
    public float MinRangeDistanceToTarget => _minRangeDistanceToTarget;
    public float MaxRangeAttackDistance => _maxRangeAttackDistance;
    public float RangeAttackAgentSpeed => _rangeAttackAgentSpeed;
    public int AmmoCount => _ammoCount;
    
    public WeaponData RangeWeaponData => _rangeWeaponData;
    
    /*
    public void Shoot(Vector3 target)
    {
        _bulletDirection = (target - _bulletStartPosition.position).normalized;
        _bullet = Instantiate(_prefabBullet,_bulletStartPosition.position, Quaternion.LookRotation(_bulletDirection));
        _bullet.Initialize(_bulletDirection, 10, 20f);
    }
    */
}
