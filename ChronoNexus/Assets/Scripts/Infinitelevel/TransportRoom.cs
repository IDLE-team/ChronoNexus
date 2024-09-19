using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportRoom : MonoBehaviour
{
    [SerializeField] private bool _isExit;
    [SerializeField] private DoorTrigger _door;
    
    public event Action OnPlayerInTransporter;
    
    private void Start()
    {
        _door.UnlockDoor();
    }

    public void SetIsExit(bool value)
    {
        _isExit = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isExit)
        {
            if (other.CompareTag("Player"))
            {
                _door.LockDoor();
                _door.CloseDoor();
                OnPlayerInTransporter?.Invoke();
            }
        }
    }

    public void LockDoor()
    {
        _door.LockDoor();
        _door.CloseDoor();
    }
    
    public void UnlockDoor()
    {
        _door.UnlockDoor();
    }
}
