using UnityEngine;
using UnityEngine.VFX;
using Zenject;

public class Attacker : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _rangeWeapon;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private VisualEffect _visualHitEffect;
    [SerializeField] private int _damage;
    [SerializeField] private CharacterController _characterController;
    
    
    private Vector3 _shootDir;
    private IInputProvider _inputProvider;
    private CharacterAnimator _animator;

    [Inject]
    private void Constuct(IInputProvider inputProvider, CharacterAnimator animator)
    {
        _inputProvider = inputProvider;
        _animator = animator;
    }

    public void StartAttack() => _animator.Attack();

    public void Fire() => _animator.Shoot();

    public void Hit()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(_attackZone.transform.position, _attackZone.Radius, _enemyLayer);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(_damage);
        }
    }

    public void Shoot()
    {
        if (_characterController.TargetLock.NearestTarget != null)
        {
            _shootDir = (_characterController.TargetLock.NearestTarget.position - _rangeWeapon.transform.position)
                .normalized;
        }
        else
        {
            _shootDir = transform.forward;
        }

        var bullet = Instantiate(_bullet, _rangeWeapon.position, Quaternion.LookRotation(_shootDir));
        bullet.SetTarget(_shootDir);
    }

    public void PlayEffect()
    {
        _visualHitEffect.gameObject.SetActive(true);
        _visualHitEffect.Play();
        _characterController.AudioController.PlayHitSound();
    }
}