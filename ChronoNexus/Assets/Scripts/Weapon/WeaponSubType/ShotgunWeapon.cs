
    using UnityEngine;

    public class ShotgunWeapon : FirearmWeapon
    {
        private float _lastFireTime;
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
            PlayWeaponAudio();

            for (int i = 0; i < 10; i++)
            {
                Vector3 spread = new Vector3(Random.Range(-5, 5), Random.Range(-15, 15), 0f);
                var spreadRotation = Quaternion.Euler(spread);
                var bullet = Instantiate(BulletPrefab, FirePosition.position, Quaternion.LookRotation(spreadRotation *_shootDir));
                bullet.Initialize(spreadRotation * _shootDir, Damage, ProjectileSpeed);
            }
            _lastFireTime = Time.time;
        }
    }
