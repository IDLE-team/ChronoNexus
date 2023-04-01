using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private float _damage;

    public float Damage => _damage;
    
    
    
    
    
    
}