using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class Bullet : MonoBehaviour, ITimeAffected
{
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField]  private Collider _collider;
    private Vector3 _shootDir;
    
    private float _damage;
    private float _baseMoveSpeed;
    private float _moveSpeed;

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
        _damage = damage;
        _shootDir = shootDirection;
        _baseMoveSpeed = speed;
        if(!isTimeSlowed)
            _moveSpeed = speed;
        else
            SlowSpeed();
    }
    

    private void Start()
    {
        Destroy(gameObject, 20);
    }

    private void FixedUpdate()
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
        Debug.Log(_moveSpeed);
        transform.position += _shootDir * (_moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Wall"))
        //{
        //    Destroy(gameObject);
        //   return;
        //}
        if(isTimeStopped)
            return;
        if ((_obstacleLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
            return;
        }
        if (!((_targetLayerMask.value & (1 << other.gameObject.layer)) > 0))
        {
            return;
        }
        if (!other.TryGetComponent<IDamagable>(out var target))
            return;
        var prevDamage = _damage;
        _damage = _damage + Random.Range(-2, 3);
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
        Debug.Log("StopTime");
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
        throw new System.NotImplementedException();
    }

    public void AcceleratedTimeAction()
    {
        throw new System.NotImplementedException();
    }
}