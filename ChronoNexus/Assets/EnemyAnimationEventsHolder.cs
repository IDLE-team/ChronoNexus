using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventsHolder : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private EnemyHumanoid _enemy;

    public void WeaponFire()
    {
        _weaponController.CurrentWeapon.Fire(_enemy.TargetFinder.Target, _enemy.transform);
    }


    
    
}
