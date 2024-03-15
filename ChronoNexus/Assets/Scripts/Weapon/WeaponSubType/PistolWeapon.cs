
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class PistolWeapon : FirearmWeapon
    {
        public override void Fire(ITargetable target, Transform holder)

        {
            if (Time.time - _lastFireTime < FireRate)
            {
                return;
            }

            if (CurrentAmmo <= 0)
            {
                Reload();
                return;
            }
Debug.Log("Target: " + target);
            if (target != null)
            {
                //if(target.GetTargetSelected())
                    _shootDir = ((target.GetTransform().position - WeaponPrefab.transform.position).normalized);
            }
            else
            {
                _shootDir = holder.forward;
            }
            var bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.LookRotation(_shootDir));
            bullet.Initialize(_shootDir, Damage, ProjectileSpeed);
            CurrentAmmo--;
            PlayWeaponAudio();
            UpdateUIWeaponValues();
            _lastFireTime = Time.time;
            if(CurrentAmmo <= 0)
                Reload();
        }

    }
