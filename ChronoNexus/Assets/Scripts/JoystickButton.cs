using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class JoystickButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private LongClickButton _button;
    [SerializeField] private HoverJoystick _joystick;
    [SerializeField] private СharacterTargetingSystem _targetLock;
    //[SerializeField] private PlayerAttacker _attacker;
    [SerializeField] private float _requiredHoldTime;
    private bool _isTargetLockPerformed;
    private float _holdTimer;

    public GameObject _targetVisualPrefab;
    public GameObject _shootVisualPrefab;

    [InputControl(layout = "Button")]
    [SerializeField]
    private string m_ControlPath;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    private void OnEnable()
    {
        _button.OnLongClicked += ActivateJoystick;
        //_button.OnClicked += StartShoot;
    }


    private void OnDisable()
    {
        _button.OnLongClicked -= ActivateJoystick;
      //  _button.OnClicked -= StartShoot;

     //   InventoryItemManager.manager.OnCharacterLinked -= SetJoystick;
    }

    private void SetJoystick()
    {
        if (!_targetLock)
        {
            var player = InventoryItemManager.manager.GetPlayer();
            if (player)
            {
                _targetLock = player.GetComponent<СharacterTargetingSystem>();
            //    _attacker = player.GetComponent<PlayerAttacker>();
            }
        }

    }

    private void Start()
    {
       // InventoryItemManager.manager.OnCharacterLinked += SetJoystick;
    }

    private PlayerInputActions _input;
    /*
    private void StartShoot()
    {
       // _attacker.StartFire();
       Debug.Log("StartFire");
       SendValueToControl(1.0f);
       SendValueToControl(0.0f);
    }
    */
    public void ActivateJoystick()
    {
        _targetVisualPrefab.SetActive(true);
        _shootVisualPrefab.SetActive(false);
        
      //  _targetLock.TurnOnStickSearch();
    }

    public void ActivateButton()
    {
        _targetVisualPrefab.SetActive(false);
        _shootVisualPrefab.SetActive(true);
      //  _targetLock.TurnOffStickSearch();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SendValueToControl(1.0f);
        SendValueToControl(0.0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}