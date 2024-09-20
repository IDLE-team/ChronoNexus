using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Transform> _entitySpawnPoints;
    [SerializeField] private List<Transform> _connectors;
    public event Action OnPlayerInRoom;

    public List<Transform> EntitySpawnPoints => _entitySpawnPoints;
    public List<Transform> Connectors => _connectors;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerInRoom?.Invoke();
        }
    }
}
