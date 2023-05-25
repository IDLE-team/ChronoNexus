using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Transform _door;
    [SerializeField] private Vector3 _doorOpenDirection;

    private Vector3 _doorClosedPosition;
    private Vector3 _doorOpenedPosition;

    [SerializeField] private bool _isLocked;

    private void Start()
    {
        _doorClosedPosition = _door.transform.position;
        _doorOpenedPosition = new Vector3(_doorClosedPosition.x + _doorOpenDirection.x, _doorClosedPosition.y + _doorOpenDirection.y, _doorClosedPosition.z + _doorOpenDirection.z);
    }

    public void OpenDoor()
    {
        _door.DOMove(_doorOpenedPosition, 1f);
    }

    public void CloseDoor()
    {
        _door.DOMove(_doorClosedPosition, 1f);
    }

    public void LockDoor()
    {
        _isLocked = true;
    }

    public void UnlockDoor()
    {
        _isLocked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (_isLocked)
            return;
        OpenDoor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        CloseDoor();
    }
}