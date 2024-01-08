using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopZoneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _zone; 
    public void SpawnZone()
    {
        Instantiate(_zone, transform.position, Quaternion.identity);
    }
}
