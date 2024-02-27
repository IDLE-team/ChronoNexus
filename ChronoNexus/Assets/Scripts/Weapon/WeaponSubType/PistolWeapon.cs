
    using UnityEngine;

    public class PistolWeapon : FirearmWeapon
    {
        public override void Fire(ITargetable target, Transform holder)
        {
            if (target != null)
            {
                _shootDir = ((target.GetTransform().position - WeaponPrefab.transform.position).normalized);
            }
            else
            {
                _shootDir = holder.forward;
            }
            var bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.LookRotation(_shootDir));
            bullet.Initialize(_shootDir, Damage, ProjectileSpeed);
        }
    }
