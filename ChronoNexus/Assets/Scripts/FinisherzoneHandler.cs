using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Zenject;
public class FinisherzoneHandler : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private WeaponData _weaponData;
    private Collider _currentTarget;
    private IFinisherable _currentFinisherTarget;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IFinisherable currentFinisherTarget))
        {
            _currentFinisherTarget = currentFinisherTarget;
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
        if(_currentFinisherTarget.Equals(null))
            return;
        if(!_currentFinisherTarget.GetFinisherableStatus())
            return;
        //_character.MainButtonController.SetFinisherButton();
        _character.CharacterEventsHolder.CallOnFinisherInteractEvent();
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
        _character.CharacterEventsHolder.CallOnShootInteractEvent();

    }
    private void OnTriggerExit(Collider other)
    {
        if(other != _currentTarget)
            return;
        
        DectivateFinisherReadyMode();
    }
    
}
