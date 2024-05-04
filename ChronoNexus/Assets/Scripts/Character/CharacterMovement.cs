using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterMovement : MonoBehaviour, ITransformable
{
    //------------------------------------------------------------------------------------//

    [Header("Character")]
    [Tooltip("Move speed of the character")]
    [SerializeField] private float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character")]
    [SerializeField] private float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement _direction")]
    [SerializeField][Range(0.0f, 0.3f)] private float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float SpeedChangeRate = 10.0f;

    [Tooltip("SmoothTargetAnimation")]
    [SerializeField] private float TargetSmoothAnimation = 2;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float enemyDetectRadius;

    [SerializeField] private float _rotationSpeed;
    //------------------------------------------------------------------------------------//

    private PlayerInputActions _input;

    public Slider speedSlider;
    public Slider rotationSlider;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI rotationSpeedText;

    [SerializeField] private Character _character;
    //SerializeField] private FloatingJoystick _joystick;

    private Camera _camera;

    private Vector3 _inputDirection;
    private Vector3 _targetDirection;
    private Vector3 _direction;

    private float _targetSpeed;
    private float _currentHorizontalSpeed;
    private float _speed;
    private float _speedOffset;

    private float _vertical;
    private float _horizontal;
    private float _inputMagnitude;

    private float _angle;
    private float _rotation;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    private float _animationBlend;
    private float _animationStrafeX;
    private float _animationStrafeZ;

    private bool _canMove = true;
    
    public Transform Transform => transform;

    [Inject]
    private void Construct(PlayerInputActions input)
    {
        _input = input;
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private Vector2 ReadMovementInput() => _input.Player.Movement.ReadValue<Vector2>();
    private Vector3 GetConvertedInputDirection(Vector2 direction) => new Vector3(direction.x, 0, direction.y);

    private void Start()
    {
        _camera = Camera.main;
    }


    private void FixedUpdate()
    {
        if(_canMove)
            Move();
    }

    private void Move()
    {
        _targetSpeed = MoveSpeed;
        Vector2 inputDirection = ReadMovementInput();
        Vector3 convertedDirection = GetConvertedInputDirection(inputDirection);

        if (inputDirection == Vector2.zero) _targetSpeed = 0.0f;

        _speedOffset = 0.1f;
        var velocity = _character.Rigidbody.linearVelocity;
        _currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

        _inputMagnitude = inputDirection.magnitude;
        //_inputDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y).normalized;

        _animationBlend = Mathf.Lerp(_animationBlend, _targetSpeed * _inputMagnitude, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        if (_currentHorizontalSpeed < _targetSpeed - _speedOffset ||
            _currentHorizontalSpeed > _targetSpeed + _speedOffset)
        {
            _speed = Mathf.Lerp(_currentHorizontalSpeed, _targetSpeed * _inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = _targetSpeed;
        }

        if (inputDirection != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(convertedDirection.x, convertedDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // _character.Rigidbody.velocity = _targetDirection.normalized * _speed;
        _character.Rigidbody.linearVelocity = new Vector3(_targetDirection.normalized.x * _speed, _character.Rigidbody.linearVelocity.y, _targetDirection.normalized.z * _speed);
        _character.Animator.StrafeZ(_animationBlend);

        if (_inputMagnitude > 0.1f)
        {
            _character.Animator.MotionSpeed(_inputMagnitude);
        }
        else
        {
            _character.Animator.MotionSpeed(1);
        }

    }
    [Delete] //Не подходит по ответственности
    public void ResetAnimationValues()
    {
        _animationStrafeX = 0;
        _animationStrafeZ = 0;
        _character.Animator.StrafeX(0);
        _character.Animator.StrafeZ(0);
    }

    
    public void LockMove()
    {
        _canMove = false;
        _character.Rigidbody.linearVelocity = Vector3.zero;
    }

    public void UnlockMove()
    {
        _canMove = true;
    }
    private void OnSpeedSliderValueChanged(float value)
    {
        MoveSpeed = value;
        speedText.text = value.ToString();
    }

    private void OnRotationSliderValueChanged(float value)
    {
        _rotationSpeed = value;
        rotationSpeedText.text = value.ToString();
    }
}