using System;
using UnityEngine;
    public abstract class FirearmWeapon : Weapon
    {
        public float ReloadSpeed;
        public float FireRate;
        public float ProjectileSpeed;
        public Bullet BulletPrefab;
        public Vector3 _shootDir;
        public float _lastFireTime;
        public abstract override void Fire(ITargetable target, Transform holder);

        public override void SetData(WeaponData data, Transform parent)
        {
            base.SetData(data, parent);
            ReloadSpeed = data.ReloadSpeed;
            FireRate = data.FireRate;
            BulletPrefab = data.BulletPrefab;
            ProjectileSpeed = data.ProjectileSpeed;
            SetFirePosition();
        }
        private void SetFirePosition()
        {
            FirePosition = WeaponPrefab.GetComponent<FirePositionHolder>().firePosition;
        }
    }