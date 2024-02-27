using UnityEngine;
    public class ThrowableWeapon : Weapon
    {

        public override void Fire(ITargetable target, Transform holder)
        {
            throw new System.NotImplementedException();
        }
    }