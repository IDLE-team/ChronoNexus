using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //------------------------------------------------------------------------------------//

    [Header("Character")]
    [Tooltip("Move speed of the character")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement _direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("SmoothTargetAnimation")]
    public float TargetSmoothAnimation = 2;

    [Tooltip("Enemy detect radius")]
    public float enemyDetectRadius;

    //------------------------------------------------------------------------------------//

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private LayerMask _lookLayer;

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

    private void FixedUpdate()
    {
        Move();
        if (_characterController._characterTargetLock.isLookAt)
        {
            if (_characterController._characterTargetLock._nearestTarget == null) return;
            transform.LookAt(new Vector3(_characterController._characterTargetLock._nearestTarget.position.x, 0, _characterController._characterTargetLock._nearestTarget.position.z));
            TargetLockSetAnimations();
        }
    }

    #region Move

    private void Move()
    {
        _targetSpeed = MoveSpeed;
        if (_joystick.Direction == Vector2.zero) _targetSpeed = 0.0f;

        _speedOffset = 0.1f;
        _currentHorizontalSpeed = new Vector3(_characterController._rigidbody.velocity.x, 0.0f, _characterController._rigidbody.velocity.z).magnitude;
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
            _targetRotation = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            _rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
        }

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _characterController._rigidbody.velocity = _targetDirection.normalized * _speed;

        #region AnimationValues

        if (_characterController._characterTargetLock.isLookAt)
        {
            _characterController._animator.SetFloat(_characterController._characterAnimation.animIDStrafeX, _animationStrafeX);
            _characterController._animator.SetFloat(_characterController._characterAnimation.animIDStrafeZ, _animationStrafeZ);
        }
        else
        {
            _characterController._animator.SetFloat(_characterController._characterAnimation.animIDStrafeZ, _animationBlend);
        }
        if (_inputMagnitude > 0.1f)
        {
            _characterController._animator.SetFloat(_characterController._characterAnimation.animIDMotionSpeed, _inputMagnitude);
        }
        else
        {
            _characterController._animator.SetFloat(_characterController._characterAnimation.animIDMotionSpeed, 1);
        }

        #endregion AnimationValues
    }

    #endregion Move

    #region TargetLock

    private void TargetLockSetAnimations()
    {
        _vertical = _targetSpeed * _joystick.Direction.y;
        _horizontal = _targetSpeed * _joystick.Direction.x;

        _direction = transform.position - _characterController._characterTargetLock._nearestTarget.position;
        _angle = Vector3.Angle(_direction, transform.forward);

        if (_direction.x > 0)
        {
            _angle = -_angle;
        }
        _angle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;

        if (_angle >= 45f && _angle <= 90f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, -_horizontal, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, _vertical, Time.deltaTime * TargetSmoothAnimation);
        }
        else if (_angle >= 90f && _angle <= 135f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, -_horizontal, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, _vertical, Time.deltaTime * TargetSmoothAnimation); ;
        }
        else if (_angle >= 135f || _angle <= -135f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, _vertical, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, _horizontal, Time.deltaTime * TargetSmoothAnimation);
        }
        else if (_angle >= -135f && _angle <= -45f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, _horizontal, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, -_vertical, Time.deltaTime * TargetSmoothAnimation);
        }
        else if (_angle >= -45f && _angle <= 45f)
        {
            _animationStrafeZ = Mathf.Lerp(_animationStrafeZ, -_vertical, Time.deltaTime * TargetSmoothAnimation);
            _animationStrafeX = Mathf.Lerp(_animationStrafeX, -_horizontal, Time.deltaTime * TargetSmoothAnimation);
        }
    }

    #endregion TargetLock

    public void ResetAnimationValues()
    {
        _characterController._animator.SetFloat(_characterController._characterAnimation.animIDStrafeX, 0);
        _characterController._animator.SetFloat(_characterController._characterAnimation.animIDStrafeZ, 0);
    }
}