using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem.OnScreen;
using Zenject;
using UnityEngine.InputSystem;

public class —haracterTargetingSystem : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;


    //[SerializeField] private Joystick _targetJoystick;
    [SerializeField] private Transform _visorPosition;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float _radius = 10f;

    [SerializeField][Min(0.1f)] private float _autoTargetRefreshDelay;
    [SerializeField][Min(0.1f)] private float _manualTargetRefreshDelay;
    [SerializeField] float _sphereCastThickness;
    public Transform Target { get; private set; }
    public Transform PreviousTarget => _previousTarget;
    public bool IsLookAt { get; private set; }
    public bool IsEnemyTargeted => _isEnemyTargeted;
    public bool IsStickSearch => _isStickSearch;

    public float DebugTestUniTask;


    private List<GameObject> _targets => Enemy.enemyList;

    private Camera _camera;
    private Character _character;

    private Transform _previousTarget;
    private Transform _nearestTarget;

    private float _nearestDistance;
    private float _targetDistance;
    private float _startSphereCastThickness;
    
    private bool _shouldFindTarget = true;
    private bool _isEnemyTargeted = false;
    private bool _isStickSearch = false;

    private PlayerInputActions _input;

    [Inject]
    private void Construct(PlayerInputActions input, CharacterAnimator animator)
    {
        Debug.Log("” ‡ÚÚ‡ÍÂ‡");
        _input = input;
        _input.Player.TargetLock.started += OnTargetLockStarted;
        _input.Player.TargetLock.canceled += OnTargetLockCanceled;


    }

    private void Start()
    {
        _camera = Camera.main;
        _character = GetComponent<Character>();
        RefreshTargetAsync().Forget();
        _startSphereCastThickness = _sphereCastThickness;
    }
    private void OnTargetLockStarted(InputAction.CallbackContext obj)
    {
        TurnOnStickSearch();
    }
    private void OnTargetLockCanceled(InputAction.CallbackContext obj)
    {
        TurnOffStickSearch();
    }
    private void FindTarget()
    {
        if (_targets.Count <= 0)
            return;

        _nearestDistance = _radius;
        _nearestTarget = null;

        foreach (var target in _targets)
        {
            if (target == null)
                continue;

            _targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (_targetDistance > _radius)
                continue;

            if (_targetDistance >= _nearestDistance)
                continue;

            _nearestDistance = _targetDistance;
            _nearestTarget = target.transform;
        }
        if (_nearestTarget == null)
            return;

        if (_nearestTarget == _previousTarget)
            return;

        if (_previousTarget != null)
            _previousTarget.GetComponent<ITargetable>().SetSelfTarget(false);

        _previousTarget = _nearestTarget;

        _nearestTarget.GetComponent<ITargetable>().SetSelfTarget(true);

        Target = _nearestTarget.transform;
        IsLookAt = true;
        _isEnemyTargeted = true;
    }

    private void ChooseTarget()
    {

        if (_targets.Count <= 0)
        {
         //   _sphereCastThickness = _startSphereCastThickness;
            return;
        }

        Vector2 inputDirection = ReadTargetMoveInput();
        if(inputDirection == Vector2.zero)
        {
            return;
        }
        Vector3 stickDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized; 

        var _targetRotation = Mathf.Atan2(stickDirection.x, stickDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        
        
        RaycastHit hit;

        Debug.DrawRay(_visorPosition.position, targetDirection.normalized*_radius, Color.green) ;

        if(Physics.SphereCast(_visorPosition.position, _sphereCastThickness, targetDirection.normalized, out hit, _radius, _targetLayer))
        {
            //_sphereCastThickness = _startSphereCastThickness;
            if (hit.transform == Target)
                return;
            Target = hit.transform;
            Target.GetComponent<ITargetable>().SetSelfTarget(true);

            if(_previousTarget != null)
                _previousTarget.GetComponent<ITargetable>().SetSelfTarget(false);

            _previousTarget = Target;
            IsLookAt = true;
        }
       // else
       // {
           // _sphereCastThickness = _startSphereCastThickness * 2f;
      //  }


    }


    private void SetEmptyTarget()
    {
        Target = null;
        IsLookAt = false;

        _previousTarget = null;
        _isEnemyTargeted = false;
        _character.Movement.ResetAnimationValues();
    }

    private async UniTaskVoid RefreshTargetAsync()
    {

        while (_shouldFindTarget)
        {
            DebugTestUniTask = Random.Range(0, 10);

            if (Target != null && Vector3.Distance(Target.transform.position, transform.position) > _radius)
            {
                Target.GetComponent<ITargetable>().SetSelfTarget(false);
                SetEmptyTarget();
            }

            if (Target == null && IsLookAt)
                SetEmptyTarget();

            if (Target == null && !_isStickSearch)
            {
                FindTarget();
            }

            else if  (_isStickSearch)
            {
                ChooseTarget();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_manualTargetRefreshDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    public void TurnOnStickSearch()
    {

        _isStickSearch = true;
    }

    public void TurnOffStickSearch()
    {
        _isStickSearch = false;
    }
    private Vector2 ReadTargetMoveInput() => _input.Player.TargetLockMove.ReadValue<Vector2>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);

    }
}
