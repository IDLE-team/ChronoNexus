using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class LiftTrigger : DoorTrigger
{
    [SerializeField] protected Transform _door2;
    
    [SerializeField] protected Vector3 _doorOpenDirection2;
    
    protected Vector3 _doorClosedPosition2;
    protected Vector3 _doorOpenedPosition2;
    
    protected override void Start()
    {
        base.Start();
        _doorClosedPosition2 = _door2.transform.localPosition;
        _doorOpenedPosition2 = new Vector3(_doorClosedPosition2.x + _doorOpenDirection2.x, _doorClosedPosition2.y + _doorOpenDirection2.y, _doorClosedPosition2.z + _doorOpenDirection2.z);
    }
    public override async UniTask OpenDoor()
    {
         _door2.DOLocalMove(_doorOpenedPosition2, 1f);
         await base.OpenDoor();
    }

    public override async UniTask CloseDoor()
    {
        _door2.DOLocalMove(_doorClosedPosition2, 1f);
        await base.CloseDoor();
    }
}
