using UnityEngine;
using UnityEngine.VFX;
using Zenject;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private Transform _rangeWeapon;

    //private Bullet _bulletPrefab;
    [SerializeField]
    private Bullet _selectedBullet;
    public Bullet SelectedBullet => _selectedBullet;

    [SerializeField]
    private Bullet[] _bulletPrefabs;

    [SerializeField]
    private AttackZone _attackZone;

    [SerializeField]
    private VisualEffect _visualHitEffect;

    [Header("Configs")]
    [SerializeField]
    private float _defaultMelleeDamage;
    public float DefaultMelleeDamage => _defaultMelleeDamage;
    private float _melleeDamage;

    [Header("Attack Speed")]
    [SerializeField]
    private float _defaultAttackInterval;
    public float AttackInterval => _defaultAttackInterval;
    [SerializeField]
    private float _rangedAttackInterval;
    public float RangedAttackInterval => _rangedAttackInterval;
    [SerializeField]
    private float _melleeAttackInterval;
    public float MelleeAttackInterval => _melleeAttackInterval;
    [SerializeField]
    private float _juggernautAttackInterval;
    public float JuggernautAttackInterval => _juggernautAttackInterval;
    [SerializeField]
    private int _juggernautAmmoCount;
    public int JuggernautAmmoCount => _juggernautAmmoCount;


    private float _attackInterval;

    [SerializeField] private bool _immortality;
    public bool Immortality => _immortality;


    
    private Enemy _enemy;
    private EnemyAnimator _animator;

   // [Inject]
   // private void Construct(IInputService inputService, CharacterAnimator animator)
   //{
        //_inputService = inputService;
        //_animator = animator;
   // }

    private void OnEnable()
    {
        _melleeDamage = _defaultMelleeDamage;
        _attackInterval = _defaultAttackInterval;
        // _inputService.Attacked += _animator.Attack;
        // _inputService.Shot += _animator.Fire;
    }
    public void ActivateImmortality(bool activate)
    {
        _immortality = activate;
    }

    public void ChangeBullet(Bullet bullet)
    {
        _selectedBullet = bullet;
    }
    public void SwapBullet()
    {
        if (_bulletPrefabs.Length > 1)
        {
            _selectedBullet = _bulletPrefabs[1];
            Debug.Log(_bulletPrefabs.Length);
        }
    }

    public void SetSpeedAttackInterval(float attackInterval)
    {
        _attackInterval = attackInterval;
    }
    public void MultiplyAttackInterval(float multi)
    {
        _attackInterval *= multi;
    }   
    public void ResetAttackInterval()
    {
        _attackInterval = _defaultAttackInterval;
    }

    public void SetDamage(float newDamage)
    {
        _melleeDamage = newDamage;
    }
    public void ResetDamage()
    {
        _melleeDamage = _defaultMelleeDamage;
    }
    public void MultiplyDamage(float multi)
    {
        _melleeDamage *= multi;
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
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_melleeDamage);
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
