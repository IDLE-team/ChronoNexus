using System;
using UnityEngine;

public class JoystickButton : MonoBehaviour
{
    [SerializeField] private LongClickButton _button;
    [SerializeField] private HoverJoystick _joystick;
    [SerializeField] private CharacterTargetLock _targetLock;
    //TODO прокинуть через Zenject
    
    private void OnEnable()
    {
        _button.OnLongClicked += ActivateJoystick;
    }

    private void OnDisable()
    {
        _button.OnLongClicked -= ActivateJoystick;
    }
    
    public void ActivateJoystick()
    {
        _joystick.gameObject.SetActive(true);
        _joystick.Initialize(_button.PointerEventData);
        _button.gameObject.SetActive(false);
        _targetLock.TurnOnStickSearch();
    }
    
    public void ActivateButton()
    {
        _joystick.gameObject.SetActive(false);
        _button.gameObject.SetActive(true);
        _targetLock.TurnOffStickSearch();
    }
}