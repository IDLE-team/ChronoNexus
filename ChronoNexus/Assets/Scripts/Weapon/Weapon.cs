using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private WeaponData data;
    
    public string WeaponName;
    public WeaponType WeaponType;
    public WeaponSubType WeaponSubType;
    
    public Transform FirePosition;
    public AudioClip WeaponSound;
    public AudioSource WeaponAudioSource;
    public GameObject WeaponPrefab;
    public WeaponAnimation WeaponAnimation;
    
    public TextMeshProUGUI WeaponUI;
    
    public float Damage;
    public float FireRate;
    public float _lastFireTime;

    public bool isFire = true;
    public bool isCoolDown;
    public abstract void Fire(ITargetable target, Transform holder);

    public virtual void AreaFire(LayerMask layerMask, int animID)
    {
        
    }
    public void StopFire()
    {
        isFire = false;
    }

    public virtual void SetData(WeaponData data, Transform parent)
    {
        this.data = data;
        WeaponName = data.WeaponName;
        WeaponType = data.WeaponType;
        WeaponSubType = data.WeaponSubType;
        WeaponSound = data.WeaponSound;
        Damage = data.Damage;
        FireRate = data.FireRate;
        WeaponAnimation = data.WeaponAnimation;
        WeaponPrefab = Instantiate(data.WeaponPrefab, parent.transform);
        SetAudioSource();
    }

    public void SetWeaponUI(TextMeshProUGUI WeaponUI)
    {
        this.WeaponUI = WeaponUI;
    }
    
    public void SetAudioSource()
    {
        WeaponAudioSource = WeaponPrefab.GetComponent<AudioSource>();
    }

    public void PlayWeaponAudio()
    {
        if (WeaponAudioSource != null)
        {
            WeaponAudioSource.PlayOneShot(WeaponSound);
        }
    }
    
    
}
