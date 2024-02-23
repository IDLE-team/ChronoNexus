using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine;
using System;
internal interface ITargetable
{
    public void SetSelfTarget(bool isActive);
    public Transform GetTransform();

    public bool GetTargetValid();

    public event Action OnTargetInvalid;
}