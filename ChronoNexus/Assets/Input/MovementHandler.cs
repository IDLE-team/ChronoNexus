using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;

public class MovementHandler : IInitializable, ITickable
{
    private IInput _input;

    private PlayerInput _playerInput;

    [Inject]
    public MovementHandler()
    {
        _playerInput = new PlayerInput();
    }
    public void Initialize()
    {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Movement.performed += Move;

        playerInputActions.Player.Fire.performed += Fire;
    }
    public void Fire(InputAction.CallbackContext context)
    {

    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }
}
