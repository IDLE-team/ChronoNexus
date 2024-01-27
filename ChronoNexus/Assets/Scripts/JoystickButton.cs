using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using Zenject;

public class JoystickButton : MonoBehaviour
{
    [SerializeField] private LongClickButton _button;
    [SerializeField] private HoverJoystick _joystick;
    [SerializeField] private СharacterTargetingSystem _targetLock;
    [SerializeField] private float _requiredHoldTime;
    [SerializeField] private Attacker _attacker;
    private bool _isTargetLockPerformed;
    private float _holdTimer;
    private void OnEnable()
    {
        _button.OnLongClicked += ActivateJoystick;
        _button.OnClicked += StartShoot;

    }

    private void OnDisable()
    {
        _button.OnLongClicked -= ActivateJoystick;
        _button.OnClicked -= StartShoot;
    }

    private PlayerInputActions _input;
    /*
    [Inject]
    private void Construct(PlayerInputActions input)
    {
        _input = input;
        _input.UI.TargetLockActivator.performed += OnTargetLockActivatorPerformed;
        _input.UI.TargetLockActivator.canceled += OnTargetLockActivatorCanceled;
    }
    private void OnTargetLockActivatorPerformed(InputAction.CallbackContext obj)
    {
        _isTargetLockPerformed = true;
    }
    private void OnTargetLockActivatorCanceled(InputAction.CallbackContext obj)
    {
        Debug.Log("Cancel"); 
        if (_isTargetLockPerformed)
        {
            _isTargetLockPerformed = false;
            Reset();
        }

        if (_holdTimer <= _requiredHoldTime)
        {
            _attacker.Shoot();
        }
        ActivateButton();

    }

    private void Update()
    {
        if (_isTargetLockPerformed)
        {

            _holdTimer += Time.deltaTime;
            Debug.Log("holdTimer: " + _holdTimer);
            if (_holdTimer >= _requiredHoldTime)
            {
                ActivateJoystick();
                Reset();
            }
        }
        if (!_isTargetLockPerformed || !(_holdTimer <= _requiredHoldTime))
        {
            return;
        }
        //Reset();
    }
    private void Reset()
    {
        _holdTimer = 0;
    }*/
    private void StartShoot()
    {
        _attacker.Shoot();
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