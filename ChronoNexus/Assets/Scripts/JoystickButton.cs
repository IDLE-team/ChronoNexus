using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class JoystickButton : MonoBehaviour
{
    [SerializeField] private LongClickButton _button;
    [SerializeField] private HoverJoystick _joystick;
    [SerializeField] private СharacterTargetingSystem _targetLock;
    [SerializeField] private PlayerAttacker _attacker;
    [SerializeField] private float _requiredHoldTime;
    [SerializeField] private WeaponController _weaponController;
    private bool _isTargetLockPerformed;
    private float _holdTimer;

    public GameObject _targetVisualPrefab;
    public GameObject _shootVisualPrefab;

    private void OnEnable()
    {
        _button.OnLongClicked += ActivateJoystick;
        _button.OnClicked += StartShoot;

        InventoryItemManager.manager.OnCharacterLinked += SetJoystick;
    }


    private void OnDisable()
    {
        _button.OnLongClicked -= ActivateJoystick;
        _button.OnClicked -= StartShoot;

        InventoryItemManager.manager.OnCharacterLinked -= SetJoystick;
    }

    private void SetJoystick()
    {
        if (!_targetLock)
        {
            var player = InventoryItemManager.manager.GetPlayer();
            if (player)
            {
                _targetLock = player.GetComponent<СharacterTargetingSystem>();
                _attacker = player.GetComponent<PlayerAttacker>();
                _weaponController = player.GetComponent<WeaponController>();
            }
        }

    }

    private PlayerInputActions _input;
    private void StartShoot()
    {
        _attacker.StartFire();
        // _weaponController.CurrentWeapon.Fire();
    }
    public void ActivateJoystick()
    {
        // _joystick.gameObject.SetActive(true);

        //_joystick.Initialize(_button.PointerEventData);
        _targetVisualPrefab.SetActive(true);
        _shootVisualPrefab.SetActive(false);

        //_joystick.enabled = true;
        //_joystick.Initialize(_button.PointerEventData);
        //_button.enabled = false;
        //  _button.gameObject.SetActive(false);
        _targetLock.TurnOnStickSearch();
    }

    public void ActivateButton()
    {
        // _joystick.gameObject.SetActive(false);
        _targetVisualPrefab.SetActive(false);
        _shootVisualPrefab.SetActive(true);
        // _joystick.enabled = false;
        //_button.enabled = true;

        // _button.gameObject.SetActive(true);
        _targetLock.TurnOffStickSearch();
    }
}