using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _targetMask;

    [SerializeField][Range(0, 360)] private float _viewAngle;
    [SerializeField][Min(0)] private float _viewRadius;

    [SerializeField][Min(0.1f)] private float _findDelay;
    public float ViewAngle => _viewAngle;
    public float ViewRadius => _viewRadius;

    public bool canSeeTarget = false;

    private ISeeker _seeker;
    private Transform _target;

    private CancellationTokenSource cancellationTokenSource;
    private CancellationToken cancellationToken;

    private bool _isSeeking;

    private void Awake()
    {
        TryGetComponent(out _seeker);
    }

    private void Update()
    {
        Debug.Log(cancellationToken.IsCancellationRequested);
    }

    private void OnEnable()
    {
        _seeker.OnSeekStart += StartSeeking;
        _seeker.OnSeekEnd += StopSeeking;
    }

    private void OnDisable()
    {
        _seeker.OnSeekStart -= StartSeeking;
        _seeker.OnSeekEnd -= StopSeeking;
    }

    private void StartSeeking()
    {
        _isSeeking = true;
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;
        FindTargetsWithDelay(_findDelay, cancellationToken).Forget();
    }

    private void StopSeeking()
    {
        _isSeeking = false;
        if (cancellationTokenSource == null)
            return;
        if (cancellationTokenSource.IsCancellationRequested == true)
            return;
        cancellationTokenSource.Cancel();
    }

    private async UniTask FindTargetsWithDelay(float delay, CancellationToken cancellationToken)
    {
        while (_isSeeking && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            FindVisibleTargets();
        }
        await UniTask.Yield();
    }

    private void FindVisibleTargets()
    {
        if (_isSeeking == false)
            return;
        var results = new Collider[30];
        var size = Physics.OverlapSphereNonAlloc(transform.position, ViewRadius, results, _targetMask);
        for (var i = 0; i < size; i++)
        {
            if (_isSeeking == false)
                break;
            if (results[i].gameObject == gameObject)
                continue;
            _target = results[i].transform;
            var dirToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) >= ViewAngle / 2)
                continue;

            var dstToTarget = Vector3.Distance(transform.position, _target.position);
            if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, _obstacleMask))
                continue;

            _seeker.Target = _target;
            _seeker.IsTargetFound = true;
        }
    }

    private void OnDestroy()
    {
        //cancellationTokenSource.Cancel();
    }

#if UNITY_EDITOR

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

#endif
}