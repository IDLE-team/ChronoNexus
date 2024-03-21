using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinisherzoneHandler : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private WeaponData _weaponData;
    private Collider _currentTarget;
    private IFinisherable _currentFinisherTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out _currentFinisherTarget))
        {
            _currentTarget = other;
            _currentFinisherTarget.OnFinisherEnded += DectivateFinisherReadyMode;
            if (!_currentFinisherTarget.GetFinisherableStatus())
            {
                _currentFinisherTarget.OnFinisherReady += ActivateFinisherReadyMode;
                return;
            }
            ActivateFinisherReadyMode();
        }
    }
    private void ActivateFinisherReadyMode()
    {
        _character.MainButtonController.SetFinisherButton();
        _character.CharacterTargetingSystem.SetTarget(_currentTarget.GetComponent<ITargetable>());
    }

    private void DectivateFinisherReadyMode()
    {
        if (_currentFinisherTarget != null)
        {
            _currentFinisherTarget.OnFinisherReady -= ActivateFinisherReadyMode;
            _currentFinisherTarget.OnFinisherEnded -= DectivateFinisherReadyMode;
            _currentTarget = null;
            _currentFinisherTarget = null;
        }
        _character.MainButtonController.SetShootButton();
    }
    private void OnTriggerExit(Collider other)
    {
        if(other != _currentTarget)
            return;
        Debug.Log("TriggerExit");
        DectivateFinisherReadyMode();
    }
    
}
