using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelStatTracker : MonoBehaviour
{
    [SerializeField] private float  _startTime;
    [SerializeField] private float _currentTime;

    [SerializeField] private int _startEnemyAmount;

    [SerializeField] private int _kills;
    private void Start()
    {
        _startTime = Time.realtimeSinceStartup;
        _startEnemyAmount = Entity.enemyList.Count;
        Debug.Log("StartEnemyAmount: " + _startEnemyAmount);
    }

    public float GetLevelWalkthroughTime()
    {
        return Time.realtimeSinceStartup - _startTime;
    }
    
    public int GetKilledEnemyAmount()
    {
        return _startEnemyAmount - Entity.enemyList.Count;
    }
}
