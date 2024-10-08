using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Transform> _entitySpawnPoints;
    [SerializeField] private List<Transform> _connectors;
    [SerializeField] private Transform _winGameTransform;
    public event Action OnPlayerInRoom;

    public List<Transform> EntitySpawnPoints => _entitySpawnPoints;
    public List<Transform> Connectors => _connectors;
    public Transform WinGameTransform => _winGameTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerInRoom?.Invoke();
        }
    }
}
