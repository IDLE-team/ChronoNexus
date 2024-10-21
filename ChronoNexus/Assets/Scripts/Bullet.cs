using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
public class Bullet : MonoBehaviour, ITimeAffected
{
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private LayerMask _currentTargetLayerMask;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private LayerMask _invertTargetLayerMask;
    [SerializeField]  private Collider _collider;
    [SerializeField] private particleColorChanger _colorChanger;
    [SerializeField] private bool _isRewindable = true;
    private Vector3 _shootDir;
    
    private float _damage;
    private float _baseMoveSpeed;
    private float _moveSpeed;

    private Vector3 _startPosition;
   [SerializeField] private float inverseDuration = 10f;
    private bool isRewinding = false; 
    public float Damage => _damage;
    
    public float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    public event Action OnTimeAffectedDestroy;

    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    public void Initialize( Vector3 shootDirection, float damage, float speed)
    {
        _currentTargetLayerMask = _targetLayerMask;
        _damage = damage;
        _shootDir = shootDirection;
        _baseMoveSpeed = speed;
        if(!isTimeSlowed || isRewinding)
            _moveSpeed = speed;
        else
            SlowSpeed();
    }
    

    private void Start()
    {
        _startPosition = transform.position;
        Destroy(gameObject, 20);
    }

    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            TimeBeforeAffectedTimer -= Time.deltaTime;
            if (TimeBeforeAffectedTimer <= 0f)
            {
                CanBeAffected = true;
            }

            if (CanBeAffected && isTimeStopped)
            {
                return;
            }
            transform.position += _shootDir * (_moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Wall"))
        //{
        //    Destroy(gameObject);
        //   return;
        //}
        //if(isTimeStopped && !isTimeRewinded)
          //  return;
        if ((_obstacleLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
            return;
        }
        if (!((_currentTargetLayerMask.value & (1 << other.gameObject.layer)) > 0))
        {
            return;
        }
        if (!other.TryGetComponent<IDamagable>(out var target))
        {
            return;
        }
        var prevDamage = _damage;
        if(_damage > 0)
            _damage = _damage + Random.Range(0, 3);
        if (_damage - prevDamage > 1)
        {
            target.TakeDamage(_damage, true);
        }
        else
        {
            target.TakeDamage(_damage, false);
        }
        OnTimeAffectedDestroy?.Invoke();
        Destroy(gameObject);
    }

    public void RealTimeAction()
    {
        _collider.enabled = true;
        _moveSpeed = _baseMoveSpeed;
        isTimeStopped = false;
        isTimeSlowed = false;
    }

    public void StopTimeAction()
    {
        _collider.enabled = false;
        isTimeStopped = true;
    }

    public void SlowTimeAction()
    {
        isTimeSlowed = true;
        if(_baseMoveSpeed == 0)
            return;
        _moveSpeed = _baseMoveSpeed - _baseMoveSpeed * UpgradeData.Instance.SlowTimePercentAfterFinisherUpgradeValue / 100;
    }

    public void SlowSpeed()
    {
        _moveSpeed = _baseMoveSpeed - _baseMoveSpeed * UpgradeData.Instance.SlowTimePercentAfterFinisherUpgradeValue / 100;

    }

    public void RewindTimeAction()
    {
        if(!_isRewindable)
            return;
        isRewinding = true;
        _currentTargetLayerMask = _invertTargetLayerMask;
        _collider.enabled = true;
        _colorChanger.applyChanges = true;
        StartCoroutine(InverseSpeed(inverseDuration));
    }
    IEnumerator InverseSpeed(float inverseDuration)
    {
        float elapsedTime = 0f;
        float startSpeed = _moveSpeed;

        while (elapsedTime < inverseDuration)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, -startSpeed, elapsedTime / inverseDuration);

            transform.position += _shootDir * (_moveSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }

    }
    public void AcceleratedTimeAction()
    {
        throw new System.NotImplementedException();
    }
}