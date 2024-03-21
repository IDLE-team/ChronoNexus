using System;
using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    
    public Weapon CreateWeapon(WeaponData data, Transform holder)
    {
        Weapon weapon;
        
        var weaponHolder = Instantiate(_prefab, holder);
        weaponHolder.name = data.WeaponName;
        
        switch (data.WeaponSubType)
        {
            case WeaponSubType.Pistol:
                var pistol = weaponHolder.AddComponent<PistolWeapon>();
                pistol.SetData(data, weaponHolder.transform);
                weapon = pistol;
                break;
                
            case WeaponSubType.Rifle:
                var rifle = weaponHolder.AddComponent<RifleWeapon>();
                rifle.SetData(data, weaponHolder.transform);
                weapon = rifle;
                break;

            case WeaponSubType.Shotgun:
                var shotgun = weaponHolder.AddComponent<ShotgunWeapon>();
                shotgun.SetData(data, weaponHolder.transform);
                weapon = shotgun;
                break;
            
            case WeaponSubType.MachineGun:
                var machineGun = weaponHolder.AddComponent<MachineGunWeapon>();
                machineGun.SetData(data, weaponHolder.transform);
                weapon = machineGun;
                break;
            
            case WeaponSubType.Sword:
                var blade = weaponHolder.AddComponent<BladeWeapon>();
                blade.SetData(data, weaponHolder.transform);
                weapon = blade;
                break;
            
            default:
               var defaultWeapon = weaponHolder.AddComponent<PistolWeapon>();
               defaultWeapon.SetData(data, weaponHolder.transform);
               weapon = defaultWeapon;
               break;
        }
        return weapon;
    }
    
}
