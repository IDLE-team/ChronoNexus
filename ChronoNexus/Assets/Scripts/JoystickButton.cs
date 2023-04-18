using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickButton : MonoBehaviour
{
    [SerializeField] private GameObject _button;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private CharacterTargetLock _targetLock;
    [SerializeField] private bool _isButton;

    public void ActivateJoystick()
    {
        _joystick.SetActive(true);
        _joystick.GetComponent<HoverJoystick>().Initialize(_button.GetComponent<LongClickButton>().PointerEventData);
        _button.SetActive(false);
        _isButton = false;
        _targetLock.TurnOnStickSearch();
    }

    public void ActivateButton()
    {
        _joystick.SetActive(false);
        _button.SetActive(true);
        _isButton = true;
        _targetLock.TurnOffStickSearch();
    }
}