
    using System.Runtime.InteropServices;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class RifleWeapon : FirearmWeapon
    {
        public override void Fire(ITargetable target, Transform holder)
        {
            if (Time.time - _lastFireTime < FireRate)
            {
                return;
            }
            _lastFireTime = Time.time;
            if(CurrentAmmo <= 0)
            {
                Reload();
                return;
            }
            FireBurst(target, holder).Forget();
        }

        async UniTask FireBurst(ITargetable target, Transform holder)
        {

            for (int i = 0; i < 3; i++)
            {
                PlayWeaponAudio();
                if (target != null)
                {
                    if(target.GetTargetSelected())
                        _shootDir = ((target.GetTransform().position - WeaponPrefab.transform.position).normalized);
                }
                else
                {
                    _shootDir = holder.forward;
                }
                var bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.LookRotation(_shootDir));
                bullet.Initialize(_shootDir, Damage, ProjectileSpeed);
                CurrentAmmo--;
                UpdateUIWeaponValues();
                if (CurrentAmmo <= 0)
                {
                    Reload();
                   await UniTask.Yield();
                   return;
                }
                if(i < 2)
                    await UniTask.Delay((int)(0.1f * 1000));
            }
        }
    }
