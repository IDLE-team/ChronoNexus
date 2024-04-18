using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageZone : MonoBehaviour, IDamagable
{

    [SerializeField]private Entity _entity;

    public void TakeDamage(float damage, bool isCritical)
    {
        _entity.TakeDamage(damage,isCritical);
    }
}
