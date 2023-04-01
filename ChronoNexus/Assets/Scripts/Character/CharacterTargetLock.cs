using System.Collections;
using UnityEngine;

public class CharacterTargetLock : MonoBehaviour
{
    [SerializeField] private LayerMask _lookLayer;

    [SerializeField] private Character _character;

    [Tooltip("Enemy detect radius")]
    [SerializeField] private float _radius = 10f;

    private Collider[] _targets;
    private Transform _previousTarget;
    private Transform _closestTarget;
    private float _closestDistance;
    private float _targetDistance;
    
    public Transform NearestTarget { get; private set; }
    public bool IsLookAt { get; private set; }

    private void Start()
    {
        StartCoroutine(RefreshTarget());
    }

    //TODO заменить на что-то более производительнее. например UniRx/UniTask
    [Fix]
    private IEnumerator RefreshTarget()
    {
        while (true)
        {
            LookAtTarget();
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    [Fix] [BadPerformance]
    private void LookAtTarget()
    {
        _targets = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), _radius, _lookLayer);
        if (_targets.Length > 0)
        {
            _closestDistance = _radius;
            NearestTarget = null;
            foreach (var target in _targets)
            {
                _targetDistance = Vector3.Distance(transform.position, target.transform.position);
                if (_targetDistance < _closestDistance)
                {
                    _closestDistance = _targetDistance;
                    _closestTarget = target.transform;
                }
            }
            NearestTarget = _closestTarget;
            if (_previousTarget != NearestTarget || _previousTarget == null)
            {
                if (_previousTarget != null)
                    _previousTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
                if (NearestTarget != null)
                {
                    NearestTarget.gameObject.GetComponent<ITargetable>().ToggleSelfTarget();
                }
                _previousTarget = NearestTarget;
            }
            IsLookAt = true;
        }
        else
        {
            if (_previousTarget != null)
            {
                _previousTarget?.gameObject.GetComponent<ITargetable>()?.ToggleSelfTarget();
            }
            _previousTarget = null;
            NearestTarget = null;
            if (IsLookAt)
            {
                _character.Movement.ResetAnimationValues();
                IsLookAt = false;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}