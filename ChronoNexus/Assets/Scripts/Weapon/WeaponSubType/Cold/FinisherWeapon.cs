using UnityEngine;
public class FinisherWeapon : ColdWeapon
{
    public override void Fire(ITargetable target, Transform holder)
    {
    }
    public override void Finisher(ITargetable target, int animID)
    {
        target.GetTargetGameObject().GetComponent<IFinisherable>()?.StartFinisher(animID);
    }
    public override void AreaFire(LayerMask layerMask, int animID)
    {
    }
    
}