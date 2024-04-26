using UnityEngine;
using UnityEngine.VFX;

public class StationaryEntityAttacker : Attacker
{
    [Header("Range Attack")]
    [SerializeField] private WeaponData _rangeWeaponData;

    [SerializeField] private float _rangeAttackInterval = 1f;


    [SerializeField] private float _minRangeDistanceToTarget = 3f;
    [SerializeField] private float _maxRangeAttackDistance = 8f;

    
    [SerializeField] private int _ammoCount = 8;

    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private Bullet _prefabBullet;

    private Bullet _bullet;
    private Vector3 _bulletDirection;
    public AudioClip shootClip;
    public AudioSource Source;
    
    
    public float RangedAttackInterval => _rangeAttackInterval;
    public float MinRangeDistanceToTarget => _minRangeDistanceToTarget;
    public float MaxRangeAttackDistance => _maxRangeAttackDistance;
    public int AmmoCount => _ammoCount;
    
    public WeaponData RangeWeaponData => _rangeWeaponData;
    
    public void Shoot(Vector3 target)
    {
        _bulletDirection = (target - _bulletStartPosition.position).normalized;
        _bullet = Instantiate(_prefabBullet,_bulletStartPosition.transform.position, Quaternion.LookRotation(_bulletDirection));
        _bullet.Initialize(_bulletDirection, 10, 20f);
        Source.PlayOneShot(shootClip);
    }
}
