using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public string WeaponName;
    public WeaponType WeaponType;
    public WeaponSubType WeaponSubType;
    public GameObject WeaponPrefab;
    public Bullet BulletPrefab;
    public AudioClip WeaponSound;
    public WeaponAnimation WeaponAnimation;
    public int MaxAmmo;
    public float Distance;
    public float Damage;
    public float ReloadSpeed;
    public float FireRate;
    public float ProjectileSpeed;

}