using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsHolder : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private Character _character;

    public void WeaponFire()
    {
        _weaponController.CurrentWeapon.Fire(_character.CharacterTargetingSystem.Target, _character.Transform);
    }


    
    
}
