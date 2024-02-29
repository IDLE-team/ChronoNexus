
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
            
            if (target != null)
            {
                _shootDir = ((target.GetTransform().position - WeaponPrefab.transform.position).normalized);
            }
            else
            {
                _shootDir = holder.forward;
            }
            _lastFireTime = Time.time;
            FireBurst().Forget();
        }
        async UniTask FireBurst()
        {
            for (int i = 0; i < 3; i++)
            {
                PlayWeaponAudio();
                Debug.Log("FirePosition: " + FirePosition);
                var bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.LookRotation(_shootDir));
                bullet.Initialize(_shootDir, Damage, ProjectileSpeed);
                await UniTask.Delay((int)(0.1f * 1000));

            }
        }
    }
