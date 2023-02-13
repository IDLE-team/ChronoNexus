using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerController
{
    //------------------------------------------------------------------------------------//

    [Header("Player")]
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

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private LayerMask _lookLayer;

    private Collider[] _colliders;

    private Transform _nearestTarget;
    private Transform _previousTarget;
    private Transform _closestTarget;

    private Vector3 _inputDirection;
    private Vector3 _targetDirection;
    private Vector3 _direction;

    private float _closestDistance;
    private float _targetDistance;

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

    private bool _isLookAt;

    private void FixedUpdate()
    {
        Move();
        CheckTargetLock();
    }

    #region Move

    private void Move()
    {
        _targetSpeed = MoveSpeed;
        if (_joystick.Direction == Vector2.zero) _targetSpeed = 0.0f;

        _speedOffset = 0.1f;
        _currentHorizontalSpeed = new Vector3(_rigidbody.velocity.x, 0.0f, _rigidbody.velocity.z).magnitude;
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

        _rigidbody.velocity = _targetDirection.normalized * _speed;

        #region AnimationValues

        if (!_hasAnimator)
        {
            return;
        }
        if (_isLookAt)
        {
            _animator.SetFloat(_playerAnimation.animIDStrafeX, _animationStrafeX);
            _animator.SetFloat(_playerAnimation.animIDStrafeZ, _animationStrafeZ);
        }
        else
        {
            _animator.SetFloat(_playerAnimation.animIDStrafeZ, _animationBlend);
        }
        if (_inputMagnitude > 0.1f)
        {
            _animator.SetFloat(_playerAnimation.animIDMotionSpeed, _inputMagnitude);
        }
        else
        {
            _animator.SetFloat(_playerAnimation.animIDMotionSpeed, 1);
        }

        #endregion AnimationValues
    }

    #endregion Move

    #region TargetLock

    public void CheckTargetLock()
    {
        if (!_isLookAt)
        {
            return;
        }

        if (_nearestTarget == null)
        {
            TurnOffTargetLock();
            return;
        }

        if (Vector3.Distance(transform.position, _nearestTarget.position) > enemyDetectRadius + 0.5)
        {
            TurnOffTargetLock();
            return;
        }
        transform.LookAt(new Vector3(_nearestTarget.position.x, 0, _nearestTarget.position.z));
        TargetLockSetAnimations();
    }

    public void LookAtTarget()
    {
        _colliders = null;
        _nearestTarget = null;
        _colliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), enemyDetectRadius, _lookLayer);
        if (_colliders.Length > 0)
        {
            _closestDistance = enemyDetectRadius;
            foreach (Collider collider in _colliders)
            {
                _targetDistance = Vector3.Distance(transform.position, collider.transform.position);
                if (_targetDistance < _closestDistance)
                {
                    _closestDistance = _targetDistance;
                    _closestTarget = collider.transform;
                }
            }
            _nearestTarget = _closestTarget;
            if (_previousTarget != _nearestTarget || _previousTarget == null)
            {
                if (_previousTarget != null)
                    _previousTarget?.gameObject.GetComponent<ITargetable>()?.ToggleSelfTarget();
                _nearestTarget?.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
                _previousTarget = _nearestTarget;
            }
            _isLookAt = true;
        }
    }

    public void TurnOffTargetLock()
    {
        _isLookAt = false;

        if (_nearestTarget != null)
            _nearestTarget.gameObject.GetComponent<ITargetable>()?.ToggleSelfTarget();

        _nearestTarget = null;
        _previousTarget = null;
        _angle = 0;
        _vertical = 0;
        _horizontal = 0;
        _animator.SetFloat(_playerAnimation.animIDStrafeX, 0);
    }

    private void TargetLockSetAnimations()
    {
        _vertical = _targetSpeed * _joystick.Direction.y;
        _horizontal = _targetSpeed * _joystick.Direction.x;

        _direction = transform.position - _nearestTarget.position;
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

    #region EnemyDetectGizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyDetectRadius);
    }

    #endregion EnemyDetectGizmos
}