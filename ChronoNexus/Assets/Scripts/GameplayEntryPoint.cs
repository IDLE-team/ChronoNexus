using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _ui;
    [SerializeField] private Transform _startPoint;

    private void Start()
    {
        Instantiate(_player, _startPoint.position, Quaternion.identity);
        Instantiate(_ui);
    }
}
