using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
    public abstract class ColdWeapon : Weapon
    {
        public float Distance;
        
        public override void SetData(WeaponData data, Transform parent)
        {
            base.SetData(data, parent);
            Distance = data.Distance;
        }
    }