using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine;
using System;
public interface ITargetable
{
    public void SetSelfTarget(bool isActive);
    public Transform GetTransform();
    public GameObject GetTargetGameObject();
    public bool GetTargetSelected();

    public bool GetTargetValid();

    public event Action OnTargetInvalid;
}