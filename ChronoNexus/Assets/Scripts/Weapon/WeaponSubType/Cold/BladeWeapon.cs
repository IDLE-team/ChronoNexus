using UnityEngine;
public class BladeWeapon : ColdWeapon
{
    public override void Fire(ITargetable target, Transform holder)
    {
    }

    public override void AreaFire(LayerMask layerMask, int animID)
    {
   //     if (Time.time - _lastFireTime < FireRate)
     //   {
     //       return;
     //   }
      //  _lastFireTime = Time.time;
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, Distance, layerMask);
        foreach (Collider collider in hitEnemies)
        {
           // collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(Damage, true);
            collider.gameObject.GetComponent<IFinisherable>()?.StartFinisher(animID);

        }
    }
}