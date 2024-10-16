using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
    public abstract class ColdWeapon : Weapon
    {
        public float Distance;
        
        public override void SetData(WeaponData data, Transform parent, bool isPlayerWeapon)
        {
            base.SetData(data, parent, isPlayerWeapon);
            Distance = data.Distance;
        }
    }