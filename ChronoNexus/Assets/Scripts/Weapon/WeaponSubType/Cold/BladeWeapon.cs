using UnityEngine;
public class BladeWeapon : ColdWeapon
{
    public override void Fire(ITargetable target, Transform holder)
    {
    }

    public override void AreaFire(LayerMask layerMask, int animID)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, Distance, layerMask);
        foreach (Collider collider in hitEnemies)
        {
            collider.gameObject.GetComponent<IFinisherable>()?.StartFinisher(animID);
        }
    }
    
}