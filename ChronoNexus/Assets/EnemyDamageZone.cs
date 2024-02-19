using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    
    [SerializeField]private Enemy _enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet>(out var bullet))
        {
            _enemy.TakeDamage(bullet.Damage);
            _enemy.TakeJuggernautDamage(bullet.Damage);
            Debug.Log("Пуля");
            return;
        }
        else if(other.TryGetComponent<Attacker>(out var attacker))
        {
            float damage = attacker.Damage;
            Debug.Log("Лезвие");
            _enemy.TakeDamage(damage);
            _enemy.TakeJuggernautDamage(damage);
            return;
        }
    }
    

}
