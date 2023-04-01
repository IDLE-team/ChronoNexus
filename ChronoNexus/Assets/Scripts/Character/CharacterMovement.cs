using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //------------------------------------------------------------------------------------//

    [Header("Character")]
    [Tooltip("Move speed of the character")]
    [SerializeField] private float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character")]
    [SerializeField] private float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement _direction")]
    [SerializeField] [Range(0.0f, 0.3f)] private float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float SpeedChangeRate = 10.0f;

    [Tooltip("SmoothTargetAnimation")]
    [SerializeField] private float TargetSmoothAnimation = 2;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float enemyDetectRadius;

    //------------------------------------------------------------------------------------//

    [SerializeField] private Character _character;
    [SerializeField] private FloatingJoystick _joystick;
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

    private void Awake()
    {
        _camera = Camera.main;
    }


    private void FixedUpdate()
    {
        Move();
        if (_character.TargetLock.IsLookAt)
        {
            if (_character.TargetLock.NearestTarget == null)
                return;
            transform.LookAt(new Vector3(_character.TargetLock.NearestTarget.position.x, 0, _character.TargetLock.NearestTarget.position.z));
            TargetLockSetAnimations();
        }
    }
    
    private void Move()
    {
        _targetSpeed = MoveSpeed;
        if (_joystick.Direction == Vector2.zero) _targetSpeed = 0.0f;

        _speedOffset = 0.1f;
        var velocity = _character.Rigidbody.velocity;
        _currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;
        _inputMagnitude = _joystick.Direction.magnitude;
        _inputDirection = new Vector3(_joystick.Direction.x, 0.0f, _joystick.Direction.y).normalized;

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

        if (_joystick.Direction != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _character.Rigidbody.velocity = _targetDirection.normalized * _speed;
        
        if (_character.TargetLock.IsLookAt)
        {
            _character.Animator.StrafeX(_animationStrafeX);
            _character.Animator.StrafeZ(_animationStrafeZ);
        }
        else
        {
            _character.Animator.StrafeZ(_animationBlend);
        }
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
        print("Reset Animations Values");
        _character.Animator.StrafeX(0);
        _character.Animator.StrafeZ(0);
    }
    
    private void TargetLockSetAnimations()
    {
        _vertical = _targetSpeed * _joystick.Direction.y;
        _horizontal = _targetSpeed * _joystick.Direction.x;

        _direction = transform.position - _character.TargetLock.NearestTarget.position;
        _angle = Vector3.Angle(_direction, transform.forward);
        
        if (_direction.x > 0)
        {
            _angle = -_angle;
        }
        _angle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;

        if (_angle is >= 45f and <= 90f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, -_horizontal, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, _vertical, Time.deltaTime * TargetSmoothAnimation);
        }
        else if (_angle is >= 90f and <= 135f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, -_horizontal, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, _vertical, Time.deltaTime * TargetSmoothAnimation); ;
        }
        else if (_angle is >= 135f or <= -135f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, _vertical, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, _horizontal, Time.deltaTime * TargetSmoothAnimation);
        }
        else if (_angle is >= -135f and <= -45f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, _horizontal, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, -_vertical, Time.deltaTime * TargetSmoothAnimation);
        }
        else if (_angle is >= -45f and <= 45f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, -_vertical, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, -_horizontal, Time.deltaTime * TargetSmoothAnimation);
        }
    }
    
}