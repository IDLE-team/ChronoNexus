using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EntityTriggerZone : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private Entity _entity;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _entity.Target != other.GetComponent<ITargetable>())
        {
            Debug.Log(other.transform.position);
            _entity.RotateTo(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && _entity.Target != other.GetComponent<ITargetable>())
        {
            Debug.Log(other.transform.position);
            _entity.RotateTo(other.transform);
        }
    }
}
