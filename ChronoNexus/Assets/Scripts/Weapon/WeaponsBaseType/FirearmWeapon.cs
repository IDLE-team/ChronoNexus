using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
    public abstract class FirearmWeapon : Weapon
    {
        public float ReloadSpeed;
        public float FireRate;
        public float ProjectileSpeed;
        public Bullet BulletPrefab;
        public Vector3 _shootDir;
        public float _lastFireTime;
        public int MaxAmmo;
        public int CurrentAmmo;

        public bool IsReloading;
        public event Action OnReload;
        public event Action OnReloadEnd;

        private void Start()
        {
            UpdateUIWeaponValues();
        }

        public override void SetData(WeaponData data, Transform parent)
        {
            base.SetData(data, parent);
            ReloadSpeed = data.ReloadSpeed;
            FireRate = data.FireRate;
            MaxAmmo = data.MaxAmmo;
            CurrentAmmo = MaxAmmo;
            BulletPrefab = data.BulletPrefab;
            ProjectileSpeed = data.ProjectileSpeed;
            SetFirePosition();
        }
        public void UpdateUIWeaponValues()
       {
           if(!WeaponUI)
               return;
           WeaponUI.text = CurrentAmmo.ToString();
       }
        public void Reload()
        {
            if(IsReloading)
                return;
            ReloadTimer().Forget();
        }

        async UniTask ReloadTimer()
        {
            CallReloadEvent();
            IsReloading = true;
            await UniTask.Delay((int) (ReloadSpeed * 1000));
            CurrentAmmo = MaxAmmo;
            IsReloading = false;
            UpdateUIWeaponValues();
            CallReloadEventEnded();
            await UniTask.Yield();
        }
        protected void CallReloadEvent()
        {
            OnReload?.Invoke();
        }
        protected void CallReloadEventEnded()
        {
            OnReloadEnd?.Invoke();
        }
        private void SetFirePosition()
        {
            FirePosition = WeaponPrefab.GetComponent<FirePositionHolder>().firePosition;
        }
    }