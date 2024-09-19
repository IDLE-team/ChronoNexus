using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Transform> _entitySpawnPoints;
    public event Action OnPlayerInRoom;

    public List<Transform> EntitySpawnPoints => _entitySpawnPoints;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerInRoom?.Invoke();
        }
    }
}
