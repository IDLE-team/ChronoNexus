using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EntityTriggerZone : MonoBehaviour
{
    [SerializeField] private Entity _entity;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _entity.Target != other.GetComponent<ITargetable>())
        {
            _entity.RotateTo(other.transform);
        }
        else if (other.CompareTag("Bullet") && _entity.Target == null)
        {
            _entity.RotateTo(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && _entity.Target != other.GetComponent<ITargetable>())
        {
            _entity.RotateTo(other.transform);
        }
    }
}
