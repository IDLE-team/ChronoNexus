using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
    public abstract class FirearmWeapon : Weapon
    {
        public float ReloadSpeed;
        public float ProjectileSpeed;
        public Bullet BulletPrefab;
        public Vector3 _shootDir;
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
            if(ReloadUI != null)
                ReloadUI.SetActive(true);
            OnReload?.Invoke();
        }
        protected void CallReloadEventEnded()
        {
            if(ReloadUI != null)
                ReloadUI.SetActive(false);
            OnReloadEnd?.Invoke();
        }
        private void SetFirePosition()
        {
            FirePosition = WeaponPrefab.GetComponent<FirePositionHolder>().firePosition;
        }
    }