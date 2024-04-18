using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Equiper : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform _holderTransform;

    public void EquipWeapon(WeaponData weapon)
    {
        if(weapon != null)
            _weaponController.ChangeWeapon(weapon, _holderTransform);
    }
    public void EquipColdWeapon(ColdWeapon weapon)
    {
        
    }
    public void EquipThrowableWeapon(ThrowableWeapon weapon)
    {
        
    }
}
