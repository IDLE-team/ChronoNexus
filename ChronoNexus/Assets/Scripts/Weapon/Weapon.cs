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
    
    public float Damage;

    public bool CanFire = true;
    public abstract void Fire(ITargetable target, Transform holder);
    
    public virtual void SetData(WeaponData data, Transform parent)
    {
        this.data = data;
        WeaponName = data.WeaponName;
        WeaponType = data.WeaponType;
        WeaponSubType = data.WeaponSubType;
        WeaponSound = data.WeaponSound;
        Damage = data.Damage;
        WeaponAnimation = data.WeaponAnimation;
        WeaponPrefab = Instantiate(data.WeaponPrefab, parent.transform);
        SetAudioSource();
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
