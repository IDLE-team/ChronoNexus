using UnityEngine;
using UnityEngine.VFX;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    /*[SerializeField]
    private Transform _rangeWeapon;*/
    [SerializeField]
    private Bullet[] _bulletPrefabs;

    [SerializeField]
    private AttackZone _attackZone;

    [SerializeField]
    private Collider _NonDamageZone;

    [SerializeField]
    private VisualEffect _visualHitEffect;
    
    [Header("Melee")]
    [SerializeField]
    private float _defaultMeleeDamage;
    public float DefaultMeleeDamage => _defaultMeleeDamage;

    [SerializeField]
    private float _melleeAttackInterval;
    public float MelleeAttackInterval => _melleeAttackInterval;

    [Header("Ranged")]
    /*[SerializeField]
    private float _defaultAttackInterval;
    public float AttackInterval => _attackInterval;*/
    [SerializeField]
    private float _rangedAttackInterval;
    public float RangedAttackInterval => _rangedAttackInterval;
    [SerializeField]
    private int _ammoCount;
    public int _AmmoCount => _ammoCount;


    [Header("Debug and temp")]
    [SerializeField]
    private float _meleeDamage;
    public float MeleeDamage => _meleeDamage;

    [SerializeField]
    private Bullet _selectedBullet;
    public Bullet SelectedBullet => _selectedBullet;

    [SerializeField] private bool _immortality;
    public bool Immortality => _immortality;







    private Enemy _enemy;


    private void OnEnable()
    {
        _enemy = GetComponent<Enemy>();
        if(_enemy.enemyType == Enemy.EnemyType.Juggernaut) 
        {
            _NonDamageZone.gameObject.SetActive(true);
        }
        else
        {
            _NonDamageZone.gameObject.SetActive(false);
        }
        _selectedBullet = _bulletPrefabs[0];
        

        //_melleeDamage = _defaultMelleeDamage;
        //_attackInterval = _defaultAttackInterval;
    }
    public void ActivateImmortality(bool activate)
    {
        _immortality = activate;
    }

    public void ChangeBullet(Bullet bullet)
    {
        _selectedBullet = bullet;
    }
    public void SwapBullet(int bulletSlot)
    {
        if (_bulletPrefabs.Length > bulletSlot - 1)
        {
            _selectedBullet = _bulletPrefabs[bulletSlot - 1];
        }
        else
        {
            Debug.Log("Bullet swap failed!");
        }
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
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_meleeDamage);
        }
    }

    [UsedInAnimator]
    public void Shoot(Vector3 target)
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 spawnPosition = position + forward * 0.5f;
        Vector3 direction = (target - transform.position).normalized;
        spawnPosition.y = spawnPosition.y + 1.5f;
        var bullet = Instantiate(_selectedBullet, spawnPosition, Quaternion.LookRotation(direction));
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
