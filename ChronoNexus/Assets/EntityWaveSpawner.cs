using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWaveSpawner : MonoBehaviour
{
    /// <summary>
    /// Transform of player model
    /// </summary>
    [Tooltip("Transform of player model.")]
    [SerializeField] private Transform _playerTransform;
    
    /// <summary>
    /// Distance between player and entity.
    /// </summary>
    [Tooltip("Distance between player and entity.")]
    [SerializeField] private float _distance = 10f;
    
    /// <summary>
    /// Time between entity spawning.
    /// </summary>
    [Tooltip("Time between entity spawning.")]
    [SerializeField] private float _spawnRate = 1f;
    
    
    
    
}
