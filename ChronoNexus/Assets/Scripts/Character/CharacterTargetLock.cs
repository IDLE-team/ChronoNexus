using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTargetLock : MonoBehaviour
{
    [SerializeField] private LayerMask _lookLayer;

    [SerializeField] private Character _character;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float _radius = 10f;

    [SerializeField][Min(0.1f)] private float _targetRefreshDelay;

    [SerializeField] private DebugEnemySpawner _enemySpawner;
    public Transform LookTarget { get; private set; }
    public bool IsLookAt { get; private set; }

    private List<GameObject> _targets = new List<GameObject>();

    private Transform _previousTarget;
    private Transform _closestTarget;

    private float _closestDistance;
    private float _targetDistance;

    private void Start()
    {
        RefreshTargetAsync().Forget();
        _targets = _enemySpawner.enemyList;
    }

    private async UniTaskVoid RefreshTargetAsync()
    {
        while (true)
        {
            FindTarget();
            await UniTask.Delay(TimeSpan.FromSeconds(_targetRefreshDelay), cancellationToken: this.GetCancellationTokenOnDestroy());
        }
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
        }
        else if (IsLookAt)
            SetEmptyTarget();
    }

    private void SetEmptyTarget()
    {
        _previousTarget = null;
        LookTarget = null;

        IsLookAt = false;
        _character.Movement.ResetAnimationValues();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}