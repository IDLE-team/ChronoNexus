using UnityEngine;

public class EntityBossReaperAttacker : Attacker
{
    
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private Bullet _prefabBullet;

    private Bullet _bullet;
    private Vector3 _bulletDirection;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField]private AudioSource _source;
    
    
    public void Shoot(Vector3 target)
    {
        _bulletDirection = (target - _bulletStartPosition.position).normalized;
        _bullet = Instantiate(_prefabBullet,_bulletStartPosition.transform.position, Quaternion.LookRotation(_bulletDirection));
        _bullet.Initialize(_bulletDirection, 10, 20f);
        _source.PlayOneShot(_shootClip);
    }
}
