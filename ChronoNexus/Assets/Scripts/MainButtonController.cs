using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class MainButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _shootButton;
    [SerializeField] private GameObject _finisherButton;
    [SerializeField] private GameObject _interactButton;

    private GameObject _currentButton;

    private CharacterEventsHolder _characterEvents;
    [Inject]
    private void Construct(CharacterEventsHolder characterEvents)
    {
        _characterEvents = characterEvents;
        _characterEvents.OnFinisherInteract += SetFinisherButton;
        _characterEvents.OnShootInteract += SetShootButton;
        _characterEvents.OnInteractionInteract += SetInteractButton;
    }
    private void Start()
    {
        if(_shootButton)
            _shootButton.SetActive(false);
        
        if(_finisherButton)
            _finisherButton.SetActive(false);
        
        if(_interactButton)
            _interactButton.SetActive(false);
        
        _currentButton = _shootButton;
        _currentButton.SetActive(true);
        
    }
    
    public void SetShootButton()
    {
        if(_shootButton != null && _currentButton == _shootButton)
            return;
        _currentButton.SetActive(false);
        _currentButton = _shootButton;
        _currentButton.SetActive(true);

    }

    public void SetFinisherButton()
    {
        if(_finisherButton != null && _currentButton == _finisherButton)
            return;
        _currentButton.SetActive(false);
        _currentButton = _finisherButton;
        _currentButton.SetActive(true);

    }

    public void SetInteractButton()
    {
        if(_interactButton != null && _currentButton == _interactButton)
            return;
        _currentButton.SetActive(false);
        _currentButton = _interactButton;
        _currentButton.SetActive(true);

    }
}
