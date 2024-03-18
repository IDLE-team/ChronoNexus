using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem.OnScreen;
using Zenject;
using UnityEngine.InputSystem;
using DG.Tweening;
public class СharacterTargetingSystem : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    public LayerMask TargetLayer => _targetLayer;

    //[SerializeField] private Joystick _targetJoystick;
    [SerializeField] private Transform _visorPosition;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float _radius = 10f;

    [SerializeField][Min(0.1f)] private float _autoTargetRefreshDelay;
    [SerializeField][Min(0.1f)] private float _manualTargetRefreshDelay;
    [SerializeField] float _sphereCastThickness;

    [SerializeField] private Transform _forwardPlayerAim;
    [SerializeField] GameObject _targetPointer;
    [SerializeField] AimRigController _aimRigController;

    public ITargetable Target { get; private set; }
    public ITargetable PreviousTarget => _previousTarget;
    public bool IsLookAt { get; private set; }
    public bool IsEnemyTargeted => _isEnemyTargeted;
    public bool IsStickSearch => _isStickSearch;

    public float DebugTestUniTask;

    Tween appearTween;
    private List<GameObject> _targets => Entity.enemyList;

    private Camera _camera;
    private Character _character;

    private ITargetable _previousTarget;
    private ITargetable _nearestTarget;

    private float _nearestDistance;
    private float _targetDistance;
    private float _startSphereCastThickness;
    private Vector3 _startTargetPointerScale;
    private bool _shouldFindTarget = true;
    private bool _isEnemyTargeted = false;
    private bool _isStickSearch = false;

    private PlayerInputActions _input;

    [Inject]
    private void Construct(PlayerInputActions input, CharacterAnimator animator)
    {
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
        _startTargetPointerScale = _targetPointer.transform.localScale;
        _aimRigController._aimTarget.position = _forwardPlayerAim.transform.position;
        _aimRigController._aimTarget.parent = _forwardPlayerAim;
    }
    private void Update()
    {
        if (_isStickSearch)
        {
            Vector2 inputDirection = ReadTargetMoveInput();
            Vector3 stickDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
            var _targetRotation = Mathf.Atan2(stickDirection.x, stickDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _targetPointer.transform.rotation = Quaternion.Euler(90, _targetRotation, 0);
            return;
        }

        if (Target != null)
        {
            Vector3 directionToEnemy = Target.GetTransform().position - _targetPointer.transform.position;

            float targetRotation = Mathf.Atan2(directionToEnemy.x, directionToEnemy.z) * Mathf.Rad2Deg;

            _targetPointer.transform.rotation = Quaternion.Euler(90, targetRotation, 0);
        }
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

            if (!target.TryGetComponent<ITargetable>(out ITargetable selectedTarget))
                continue;
            
            if (!selectedTarget.GetTargetValid())
            {
                continue;
            }

            _targetDistance = Vector3.Distance(transform.position, selectedTarget.GetTransform().position);

            if (_targetDistance > _radius)
                continue;

            if (_targetDistance >= _nearestDistance)
                continue;

            _nearestDistance = _targetDistance;
            _nearestTarget = selectedTarget;
        }
        if (_nearestTarget == null)
            return;

        if (_nearestTarget == _previousTarget)
            return;

        if (_previousTarget != null)
            _previousTarget.SetSelfTarget(false);

        _previousTarget = _nearestTarget;

        _nearestTarget.SetSelfTarget(true);

        if (Target != null)
        {
            Target.OnTargetInvalid -= SetEmptyTarget;
        }
        Target = _nearestTarget;
        ShowTargetPointer();
        IsLookAt = true;

        _aimRigController._aimTarget.position = Target.GetTransform().position;
        _aimRigController._aimTarget.parent = Target.GetTransform();
        Target.OnTargetInvalid += SetEmptyTarget;

        _aimRigController.SetSmoothWeight(1);
        _isEnemyTargeted = true;
    }

    private void ChooseTarget()
    {
        RaycastHit hit;
        if (_targets.Count <= 0)
        {
            //   _sphereCastThickness = _startSphereCastThickness;
            Debug.Log("NoTargets");
            return;
        }

        Vector2 inputDirection = ReadTargetMoveInput();
        if (inputDirection == Vector2.zero)
        {
            Debug.Log("Zero");
            return;
        }
        Vector3 stickDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;

        var _targetRotation = Mathf.Atan2(stickDirection.x, stickDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;


        Debug.DrawRay(_visorPosition.position, targetDirection.normalized * _radius, Color.green);


        if (Physics.SphereCast(_visorPosition.position, _sphereCastThickness, targetDirection.normalized, out hit, _radius, _targetLayer))
        {
            //_sphereCastThickness = _startSphereCastThickness;
            if (!hit.transform.gameObject.TryGetComponent<ITargetable>(out ITargetable selectedTarget))
                return;
            if (selectedTarget == Target)
                return;

            
            if (!selectedTarget.GetTargetValid())
            {
                return;
            }
            if (Target != null)
            {
                Target.OnTargetInvalid -= SetEmptyTarget;
            }
            Target = selectedTarget;
            Target.SetSelfTarget(true);
            if (_previousTarget != null)
                _previousTarget.SetSelfTarget(false);

            _previousTarget = Target;
            _aimRigController._aimTarget.position = Target.GetTransform().position;
            _aimRigController._aimTarget.parent = Target.GetTransform();

            _aimRigController.SetSmoothWeight(1);

            Target.OnTargetInvalid += SetEmptyTarget;

            IsLookAt = true;
        }


    }


    private void SetEmptyTarget()
    {
        Debug.Log("SetEmptyCalled");
        //_aimRigController._aimTarget.SetParent(null, true);
        _aimRigController._aimTarget.position = _forwardPlayerAim.transform.position;
        _aimRigController._aimTarget.parent = _forwardPlayerAim;
        _aimRigController.SetSmoothWeight(0);

        Target = null;
        IsLookAt = false;

        _previousTarget = null;
        _isEnemyTargeted = false;

        // _character.Movement.ResetAnimationValues();


        if (!_isStickSearch)
            HideTargetPointer();

        //_character.Movement.ResetAnimationValues();
    }
    private void ShowTargetPointer()
    {
        _targetPointer.SetActive(true);
        _targetPointer.transform.localScale = Vector3.zero;
        if (appearTween != null)
        {
            appearTween.Kill();
        }
        appearTween = _targetPointer.transform.DOScale(_startTargetPointerScale, 0.2f);

    }
    private void HideTargetPointer()
    {
        if (appearTween != null)
        {
            appearTween.Kill();
        }
        appearTween = _targetPointer.transform.DOScale(0, 0.2f).OnComplete(() => _targetPointer.SetActive(false));

    }
    private async UniTaskVoid RefreshTargetAsync()
    {

        while (_shouldFindTarget)
        {
            DebugTestUniTask = Random.Range(0, 10);

            if (Target != null && Vector3.Distance(Target.GetTransform().position, transform.position) > _radius)
            {
                Target.SetSelfTarget(false);
                SetEmptyTarget();
            }

            if (Target == null && IsLookAt)
                SetEmptyTarget();

            if (Target == null && !_isStickSearch)
            {
                FindTarget();
            }

            else if (_isStickSearch)
            {
                ChooseTarget();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_manualTargetRefreshDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    public void TurnOnStickSearch()
    {
        _isStickSearch = true;
        ShowTargetPointer();

    }

    public void TurnOffStickSearch()
    {
        _isStickSearch = false;
        if (Target == null)
        {
            HideTargetPointer();
        }
    }
    private Vector2 ReadTargetMoveInput() => _input.Player.TargetLockMove.ReadValue<Vector2>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);

    }
}
