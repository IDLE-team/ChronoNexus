using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDamageZone : MonoBehaviour, IDamagable, IFinisherable
{

    [SerializeField]private Entity _entity;
    
    public void TakeDamage(float damage, bool isCritical)
    {
        _entity.TakeDamage(damage,isCritical);
    }

    public void StartFinisher(int id)
    {
        _entity.StartFinisher(id);
    }

    public bool GetFinisherableStatus()
    {
        return _entity.GetFinisherableStatus();
    }

    public event Action OnFinisherReady;
    public event Action OnFinisherEnded;
    public event Action OnFinisherInvalid;
}
