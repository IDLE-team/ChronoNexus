using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using Zenject;

public class MainButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _shootButton;
    [SerializeField] private GameObject _finisherButton;
    [SerializeField] private GameObject _interactButton;

    [SerializeField] private List<GameObject> _additionalButtons = new List<GameObject>();


    private GameObject _currentButton;
    public GameObject CurrentButton => _currentButton;

    private CharacterEventsHolder _characterEvents;
    [Inject]
    private void Construct(CharacterEventsHolder characterEvents)
    {
        _characterEvents = characterEvents;
        _characterEvents.OnFinisherInteract += SetFinisherButton;
        _characterEvents.OnShootInteract += SetShootButton;
        _characterEvents.OnInteractionInteract += SetInteractButton;
        _characterEvents.OnHideInteract += HideInteractButton;
        _characterEvents.OnHideAdditional += HideAdditionalButtons;
        _characterEvents.OnShowAdditional += ShowAdditionalButtons;
        
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
        Debug.Log("SetShoot");
        _currentButton.SetActive(true);
        if(_shootButton == null || _currentButton == _shootButton)
            return;
        if (_currentButton != null) 
            _currentButton.SetActive(false);
        _currentButton = _shootButton;
        _currentButton.SetActive(true);

    }

    public void SetFinisherButton()
    {
        if(_finisherButton == null || _currentButton == _finisherButton)
            return;
        if (_currentButton != null) 
            _currentButton.SetActive(false);
        _currentButton = _finisherButton;
        _currentButton.SetActive(true);
        _currentButton.GetComponent<OnScreenButton>().enabled = true;

    }

    public void SetInteractButton()
    {
        if(_interactButton == null || _currentButton == _interactButton)
            return;
        if (_currentButton != null) 
            _currentButton.SetActive(false);
        _currentButton = _interactButton;
        _currentButton.SetActive(true);
        _currentButton.GetComponent<OnScreenButton>().enabled = true;

    }
    public void HideInteractButton()
    {
        _currentButton.SetActive(false);
    }

    public void HideAdditionalButtons()
    {
        for (int i = 0; i < _additionalButtons.Count; i++)
        {
            _additionalButtons[i].GetComponent<OnScreenButton>().enabled = false;
               // SetActive(false);
        }
    }
    public void ShowAdditionalButtons()
    {
        for (int i = 0; i < _additionalButtons.Count; i++)
        {
            _additionalButtons[i].GetComponent<OnScreenButton>().enabled = true;
        }
    }
}
