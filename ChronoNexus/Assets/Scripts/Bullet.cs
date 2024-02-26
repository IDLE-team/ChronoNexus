using System;
using UnityEngine;

//TODO подумать насчет временем полёта пули / время уничтожения
//TODO изменить setTarget
public class Bullet : MonoBehaviour, ITimeAffected
{
    [SerializeField][Min(1)] private int _damage = 10;
    public int Damage => _damage;
    [SerializeField] private float _moveSpeed;

    [SerializeField] private LayerMask _obstacleLayerMask;

    private Vector3 _shootDir;

    public float TimeBeforeAffectedTimer;
    private bool CanBeAffected;
    public event Action OnTimeAffectedDestroy;

    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    
    public void SetTarget(Vector3 shootDirection)
    {
      //  Debug.Log("ShootDir: " + shootDirection);
        _shootDir = shootDirection;
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
        if (CanBeAffected && TimeManager.instance.IsTimeStopped && !isTimeStopped)
        {
            return;
        }
        else if (CanBeAffected && TimeManager.instance.IsTimeSlowed && !isTimeSlowed)
        {
            transform.position += _shootDir * (_moveSpeed * 0.1f * Time.deltaTime);
            return;
        }
        transform.position += _shootDir * (_moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Wall"))
        //{
        //    Destroy(gameObject);
        //   return;
        //}
        
        if ((_obstacleLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
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
        isTimeStopped = false;
        isTimeSlowed = false;
    }

    public void StopTimeAction()
    {
        isTimeStopped = true;
    }

    public void SlowTimeAction()
    {
        isTimeSlowed = true;
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