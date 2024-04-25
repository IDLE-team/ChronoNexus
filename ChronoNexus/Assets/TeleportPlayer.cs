using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform teleportPoint;
    [SerializeField] private Image _fade;
    private Transform _player;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            _fade.DOFade(1, 0.5f).OnComplete(() => TeleportPlayerToPoint());
        }
    }

    private void TeleportPlayerToPoint()
    {
        _player.position = teleportPoint.position;
        _fade.DOFade(0, 0.5f).SetDelay(0.5f);

    }
}
