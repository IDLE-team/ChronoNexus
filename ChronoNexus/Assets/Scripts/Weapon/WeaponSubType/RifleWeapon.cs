
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class RifleWeapon : FirearmWeapon
    {
        public override void Fire(ITargetable target, Transform holder)
        {
            if(!CanFire)
                return;
            
            if (target != null)
            {
                _shootDir = ((target.GetTransform().position - WeaponPrefab.transform.position).normalized);
            }
            else
            {
                _shootDir = holder.forward;
            }

            FireBurst().Forget();
        }
        async UniTask FireBurst()
        {
            CanFire = false;
            for (int i = 0; i < 3; i++)
            {
                var bullet = Instantiate(BulletPrefab,FirePosition.position, Quaternion.LookRotation(_shootDir));
                bullet.Initialize(_shootDir, Damage, ProjectileSpeed);
                await UniTask.Delay((int)(FireRate * 1000));

            }

            CanFire = true;
        }
    }
