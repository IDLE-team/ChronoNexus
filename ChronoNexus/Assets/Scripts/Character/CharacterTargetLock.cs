using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterTargetLock : MonoBehaviour
{
    [SerializeField] private LayerMask _lookLayer;

    [SerializeField] private Character _character;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float _radius = 10f;

    [SerializeField][Min(0.1f)] private float _autoTargetRefreshDelay;
    [SerializeField][Min(0.1f)] private float _manualTargetRefreshDelay;

    [SerializeField] private float _angleThreshold;
    [SerializeField] private Joystick _targetJoystick;
    [SerializeField] private DebugEnemySpawner _enemySpawner;
    public Transform LookTarget { get; private set; }
    public bool IsLookAt { get; private set; }
    public float DebugTestUniTask;
    private List<GameObject> _targets = new List<GameObject>();

    private Transform _previousTarget;

    public Transform PreviousTarget => _previousTarget;
    private Transform _closestTarget;

    private float _previousAngle;
    private float _closestDistance;
    private float _targetDistance;

    private bool _isEnemyTargeted = false;
    private bool _isStickSearch = false;

    public bool IsEnemyTargeted => _isEnemyTargeted;
    public bool IsStickSearch => _isStickSearch;

    private void Start()
    {
        RefreshTargetAsync().Forget();
        _targets = _enemySpawner.enemyList;
    }

    private void FindTarget()
    {
        if (_targets.Count > 0)
        {
            _closestDistance = _radius;
            _closestTarget = null;

            foreach (var target in _targets)
            {
                _targetDistance = Vector3.Distance(transform.position, target.transform.position);

                if (_targetDistance > _radius)
                    continue;

                if (_targetDistance < _closestDistance)
                {
                    _closestDistance = _targetDistance;
                    _closestTarget = target.transform;
                }
            }

            if (_closestTarget == null && IsLookAt)
            {
                if (_previousTarget != null)
                {
                    _previousTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
                }
                SetEmptyTarget();
                return;
            }

            if (_closestTarget == _previousTarget)
                return;

            if (_previousTarget != null)
                _previousTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();

            LookTarget = _closestTarget;
            _previousTarget = _closestTarget;

            LookTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
            IsLookAt = true;
            _isEnemyTargeted = true;
        }
        else if (IsLookAt)
            SetEmptyTarget();
    }

    public void TurnOnStickSearch()
    {
        _isStickSearch = true;
    }

    public void TurnOffStickSearch()
    {
        _isStickSearch = false;
    }

    public void ChooseTarget()
    {
        if (_enemySpawner.enemyList.Count <= 0 && IsLookAt)
        {
            SetEmptyTarget();
            return;
        }
        _previousAngle = _angleThreshold;
        for (int i = 0; i < _enemySpawner.enemyList.Count; i++)
        {
            if (!_enemySpawner.enemyList[i].gameObject.activeInHierarchy)
                continue;
            if (Vector3.Distance(_enemySpawner.enemyList[i].transform.position, transform.position) > _radius)
                continue;

            float angle = GetAngle(_enemySpawner.enemyList[i]);

            if (angle > _angleThreshold)
                continue;

            if (_enemySpawner.enemyList[i] == _previousTarget)
                continue;
            if (i != 0)
            {
                if (Mathf.Abs(_previousAngle - angle) <= 5)
                {
                    var current = Vector3.Distance(_enemySpawner.enemyList[i].transform.position, transform.position);
                    var previous = Vector3.Distance(_enemySpawner.enemyList[i - 1].transform.position, transform.position);
                    if (current > previous)
                    {
                        _previousAngle = angle;
                        continue;
                    }
                }
            }

            _previousAngle = angle;

            _enemySpawner.enemyList[i].gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
            if (_previousTarget != null)
            {
                _previousTarget.GetComponent<ITargetable>().ToggleSelfTarget();
            }
            IsLookAt = true;

            LookTarget = _enemySpawner.enemyList[i].transform;

            _previousTarget = _enemySpawner.enemyList[i].transform;
            _previousAngle = angle;

            _isEnemyTargeted = true;
        }
    }

    private float GetAngle(GameObject enemy)
    {
        Vector3 stickDirection = new Vector3(_targetJoystick.Direction.x, 0, _targetJoystick.Direction.y).normalized;
        Vector3 enemyDirection = (enemy.transform.position - transform.position);

        enemyDirection = new Vector3(enemyDirection.x, 0, enemyDirection.z);

        float angle = Vector3.Angle(enemyDirection, stickDirection);
        //Debug.Log(enemy.name + "  " + angle);
        return angle;
    }

    private void SetEmptyTarget()
    {
        _previousTarget = null;
        LookTarget = null;
        _isEnemyTargeted = false;
        IsLookAt = false;
        _character.Movement.ResetAnimationValues();
    }

    private async UniTaskVoid RefreshTargetAsync()
    {
        while (true)
        {
            DebugTestUniTask = Random.Range(0, 10);
            if (LookTarget != null && Vector3.Distance(LookTarget.transform.position, transform.position) > _radius)
            {
                Debug.Log(Vector3.Distance(LookTarget.transform.position, transform.position));
                LookTarget.GetComponent<ITargetable>().ToggleSelfTarget();
                SetEmptyTarget();
            }
            if (LookTarget == null)
            {
                _isEnemyTargeted = false;
            }

            if (!_isEnemyTargeted && !_isStickSearch)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}