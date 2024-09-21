using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TransportRoom : MonoBehaviour
{
    [SerializeField] private bool _isExit;
    [SerializeField] private DoorTrigger _door;
    [SerializeField] private Transform _connector;
    public Transform Connector => _connector;
    public event Action OnPlayerInTransporter;
    
    private void Start()
    {
        _door.UnlockDoor();
    }

    public void SetIsExit(bool value)
    {
        _isExit = value;
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (_isExit)
        {
            if (other.CompareTag("Player"))
            {
                _door.LockDoor();
                await _door.CloseDoor();
                OnPlayerInTransporter?.Invoke();
            }
        }
    }

    public void LockDoor()
    {
        _door.LockDoor();
    }
    
    public void UnlockDoor()
    {
        _door.UnlockDoor();
    }
}
