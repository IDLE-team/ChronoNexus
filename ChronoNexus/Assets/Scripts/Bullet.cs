using Language.Lua;
using System;
using UnityEngine;

//TODO подумать насчет временем полёта пули / время уничтожения
//TODO изменить setTarget
public class Bullet : MonoBehaviour, ITimeAffected
{
    [SerializeField][Min(1)] private int _damage = 10;
    [SerializeField] private float _moveSpeed;

    private Vector3 _shootDir;

    public float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    private bool IsStopped;

    public event Action OnTimeAffectedDestroy;

    public bool isTimeStopped { get; set ; }
    public bool isTimeAccelerated { get ; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    public void SetTarget(Vector3 shootDirection)
    {
        _shootDir = shootDirection;
    }

    private void Start()
    {
        Destroy(gameObject, 20);
    }

    private void FixedUpdate()
    {
        TimeBeforeAffectedTimer -= Time.deltaTime;
        if(TimeBeforeAffectedTimer <= 0f)
        {
            CanBeAffected = true;
        }
        if(CanBeAffected && TimeManager.instance.IsTimeStopped && !IsStopped)
        {
            return;
        }
        transform.position += _shootDir * (_moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }
        if (!other.TryGetComponent<IDamagable>(out var target))
            return;
        target.TakeDamage(_damage);
        OnTimeAffectedDestroy?.Invoke();
        Destroy(gameObject);
    }

    public void RealTimeAction()
    {
        IsStopped = false;
    }

    public void StopTimeAction()
    {
        IsStopped = true;
    }

    public void SlowTimeAction()
    {
        throw new System.NotImplementedException();
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