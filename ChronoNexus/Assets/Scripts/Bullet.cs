using UnityEngine;


//TODO подумать насчет временем полёта пули / время уничтожения
//TODO изменить setTarget
public class Bullet : MonoBehaviour
{
    [SerializeField] [Min(1)] int _damage = 10;
    [SerializeField] private float _moveSpeed;

    private Vector3 _shootDir;
    
    
    public void SetTarget(Vector3 shootDirection)
    {
        _shootDir = shootDirection;
    }

    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void FixedUpdate()
    {
        transform.position += _shootDir * (_moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IDamagable>(out var target)) 
            return;
        target.TakeDamage(_damage);
        Destroy(gameObject);
    }
}