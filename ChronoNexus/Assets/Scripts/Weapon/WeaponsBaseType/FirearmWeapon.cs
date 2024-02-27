using System;
using UnityEngine;
    public abstract class FirearmWeapon : Weapon
    {
        public float ReloadSpeed;
        public float FireRate;
        public float ProjectileSpeed;
        public Bullet BulletPrefab;
        public Vector3 _shootDir;
        public abstract override void Fire(ITargetable target, Transform holder);

        public override void SetData(WeaponData data)
        {
            base.SetData(data);
            ReloadSpeed = data.ReloadSpeed;
            FireRate = data.FireRate;
            BulletPrefab = data.BulletPrefab;
            ProjectileSpeed = data.ProjectileSpeed;
        }
        public void SetFirePosition()
        {
            FirePosition = WeaponPrefab.GetComponent<FirePositionHolder>().firePosition;
        }
    }