using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTargetLock : MonoBehaviour
{
    [HideInInspector] public Transform _nearestTarget { get; private set; }
    [HideInInspector] public bool isLookAt { get; private set; }

    [SerializeField] private LayerMask _lookLayer;
    [SerializeField] private CharacterController _characterController;

    [Tooltip("Enemy detect radius")]
    public float enemyDetectRadius;

    private Collider[] _colliders;

    private Transform _previousTarget;
    private Transform _closestTarget;

    private float _closestDistance;
    private float _targetDistance;

    private void Start()
    {
        StartCoroutine(RefreshTarget());
    }

    public void LookAtTarget()
    {
        _colliders = null;
        _colliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), enemyDetectRadius, _lookLayer);
        if (_colliders.Length > 0)
        {
            _closestDistance = enemyDetectRadius;
            foreach (Collider collider in _colliders)
            {
                if (collider == null)
                {
                    continue;
                }
                _targetDistance = Vector3.Distance(transform.position, collider.transform.position);
                if (_targetDistance < _closestDistance)
                {
                    _closestDistance = _targetDistance;
                    _closestTarget = collider.transform;
                }
                _nearestTarget = null;
            }
            _nearestTarget = _closestTarget;
            if (_previousTarget != _nearestTarget || _previousTarget == null)
            {
                if (_previousTarget != null)
                    _previousTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
                if (_nearestTarget != null)
                {
                    _nearestTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
                }
                _previousTarget = _nearestTarget;
            }
            isLookAt = true;
        }
        else
        {
            if (_previousTarget != null)
            {
                _previousTarget?.gameObject.GetComponent<ITargetable>()?.ToggleSelfTarget();
            }
            _previousTarget = null;
            _nearestTarget = null;
            if (isLookAt)
            {
                _characterController.CharacterMovement.ResetAnimationValues();
                isLookAt = false;
            }
        }
    }

    private IEnumerator RefreshTarget()
    {
        while (true)
        {
            LookAtTarget();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyDetectRadius);
    }
}