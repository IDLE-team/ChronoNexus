using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG;
using DG.Tweening;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] protected Transform _door;
    [SerializeField] protected Vector3 _doorOpenDirection;

    protected Vector3 _doorClosedPosition;
    protected Vector3 _doorOpenedPosition;

    [SerializeField] protected bool _isLocked;

    protected virtual void Start()
    {
        _doorClosedPosition = _door.transform.localPosition;
        _doorOpenedPosition = new Vector3(_doorClosedPosition.x + _doorOpenDirection.x, _doorClosedPosition.y + _doorOpenDirection.y, _doorClosedPosition.z + _doorOpenDirection.z);
    }

    public virtual async UniTask OpenDoor()
    {
        await _door.DOLocalMove(_doorOpenedPosition, 1f).AsyncWaitForCompletion();
    }

    public virtual async UniTask CloseDoor()
    {
        await _door.DOLocalMove(_doorClosedPosition, 1f).AsyncWaitForCompletion();
    }

    public void LockDoor()
    {
        _isLocked = true;
    }

    public  void UnlockDoor()
    {
        _isLocked = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy"))
            return;
        if (_isLocked)
            return;
        OpenDoor();
    }

    protected void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy"))
            return;
        if (_isLocked)
            return;
        CloseDoor();
    }
}