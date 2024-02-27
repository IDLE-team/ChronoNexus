using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private WeaponData data;
    
    public string WeaponName;
    public WeaponType WeaponType;
    public WeaponSubType WeaponSubType;

    public GameObject WeaponPrefab;
    public WeaponAnimation WeaponAnimation;

    public Transform FirePosition;
    public float Damage;

    public bool CanFire = true;
    public abstract void Fire(ITargetable target, Transform holder);
    
    public virtual void SetData(WeaponData data)
    {
        this.data = data;
        WeaponName = data.WeaponName;
        WeaponType = data.WeaponType;
        WeaponSubType = data.WeaponSubType;
        WeaponPrefab = data.WeaponPrefab;
        Damage = data.Damage;
        WeaponAnimation = data.WeaponAnimation;
    }
}
