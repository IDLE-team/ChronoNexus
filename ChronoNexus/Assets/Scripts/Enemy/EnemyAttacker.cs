using UnityEngine;
using UnityEngine.VFX;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;
    [SerializeField]
    private Transform _shootPosition;
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
    private float _defaultMeleeAttackInterval;
    public float DefaultMeleeAttackInterval => _defaultMeleeAttackInterval;
    

    [Header("Ranged")]
    [SerializeField]
    private float _defaultRangedAttackInterval;
    public float DefaultRangedAttackInterval => _defaultRangedAttackInterval;
    
    [SerializeField]
    private int _ammoCount;
    public int _AmmoCount => _ammoCount;


    [Header("Temp")]

    [SerializeField]
    private float _rangedAttackInterval;
    public float RangedAttackInterval => _rangedAttackInterval;

    [SerializeField]
    private float _meleeAttackInterval;
    public float MeleeAttackInterval => _meleeAttackInterval;

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
        _selectedBullet = _bulletPrefabs[0];

        _rangedAttackInterval = _defaultRangedAttackInterval;
        _meleeAttackInterval = _defaultMeleeAttackInterval;
        _meleeDamage = _defaultMeleeDamage;
        

        _enemy = GetComponent<Enemy>();

        if(_enemy.enemyType == Enemy.EnemyType.Juggernaut) 
        {
            _NonDamageZone.gameObject.SetActive(true);
        }
        else
        {
            _NonDamageZone.gameObject.SetActive(false);
        }

        
        

        //_melleeDamage = _defaultMelleeDamage;
        //_attackInterval = _defaultAttackInterval;
    }
    public void ActivateImmortality(bool activate)
    {
        _immortality = activate;
    }



    public void SetRangedAttackInterval(float interval)
    {
        _rangedAttackInterval = interval;
    }
    public void ResetRangedAttackInterval()
    {
        _rangedAttackInterval = _defaultRangedAttackInterval;
    }
    public void SetMeleeAttackInterval(float interval)
    {
        _meleeAttackInterval = interval;
    }
    public void ResetMeleeAttackInterval()
    {
        _meleeAttackInterval = _defaultMeleeAttackInterval;
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
        Vector3 direction = (target - _shootPosition.position).normalized;
        var bullet = Instantiate(_selectedBullet, _shootPosition.position, Quaternion.LookRotation(direction));
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
