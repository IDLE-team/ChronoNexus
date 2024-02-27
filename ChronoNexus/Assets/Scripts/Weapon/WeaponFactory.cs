using System;
using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    
    public Weapon CreateWeapon(WeaponData data, Transform holder)
    {
        GameObject spawnedWeapon; 
        switch (data.WeaponSubType)
        {
            case WeaponSubType.Pistol:
               spawnedWeapon = Instantiate(_prefab, holder);
               spawnedWeapon.name = data.WeaponName;
               var pistolWeapon = spawnedWeapon.AddComponent<PistolWeapon>();
               pistolWeapon.SetData(data);
               pistolWeapon.WeaponPrefab = Instantiate(pistolWeapon.WeaponPrefab, spawnedWeapon.transform);
               pistolWeapon.SetFirePosition();
               return pistolWeapon;



            case WeaponSubType.Rifle:
                spawnedWeapon = Instantiate(_prefab, holder);
                spawnedWeapon.name = data.WeaponName;
                var rifleWeapon = spawnedWeapon.AddComponent<RifleWeapon>();
                rifleWeapon.SetData(data);
                rifleWeapon.WeaponPrefab = Instantiate(rifleWeapon.WeaponPrefab, spawnedWeapon.transform);
                rifleWeapon.SetFirePosition();

                return  rifleWeapon;
                     default:
                 throw new ArgumentOutOfRangeException(nameof(data), data, null);
        }
    }
    
}
