using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTargeting : MonoBehaviour
{
    private AimRigController _aimRigController;
    [SerializeField]private GameObject _aimTransform;
    private void Start()
    {
        _aimRigController = GetComponent<AimRigController>();
    }

    public void SetTargetParent(Transform parent)
    {
        _aimTransform.transform.position = parent.transform.position;
        _aimTransform.transform.SetParent(parent); 
        //_aimTransform.transform.position = Vector3.zero;
    }
}
