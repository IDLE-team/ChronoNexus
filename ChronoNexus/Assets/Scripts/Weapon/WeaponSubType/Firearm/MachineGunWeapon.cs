    using Cysharp.Threading.Tasks;
    using UnityEngine;
    public class MachineGunWeapon : FirearmWeapon
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
            _lastFireTime = Time.time;
            isFire = true;
            FireBurst(target, holder).Forget();
        }
        async UniTask FireBurst(ITargetable target, Transform holder)
        {
            while (isFire == true && CurrentAmmo > 0)
            {
                PlayWeaponAudio();
                if (target != null)
                {
                    if(target.GetTargetSelected())
                        _shootDir = ((target.GetTransform().position - WeaponPrefab.transform.position).normalized);
                    else
                        _shootDir = holder.forward;
                }
                else
                {
                    _shootDir = holder.forward;
                }
                var bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.LookRotation(_shootDir));
                bullet.Initialize(_shootDir, Damage, ProjectileSpeed);
                CurrentAmmo--;
                UpdateUIWeaponValues();
                Debug.Log("isFire: " + isFire);
                await UniTask.Delay((int) (0.1f * 1000));
            }

            if (CurrentAmmo <= 0)
            {
                Reload();
            }
            StopFire();
            await UniTask.Yield();
        }
    }
