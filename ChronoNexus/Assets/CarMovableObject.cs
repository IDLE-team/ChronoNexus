using System;
using UnityEngine;
using UnityEngine.UI;

public class CarMovableObject : MonoBehaviour, ITimeAffected
{

    private Rigidbody _rigidbody;
    private Vector3 _positionToEnd;
    private float _speed;

    private float _timer;
    private float _endTimer;


    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    public event Action OnTimeAffectedDestroy;

    private BoxCollider _collider;
    private bool CanBeAffected;

    public float TimeBeforeAffectedTimer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<BoxCollider>();
    }

    public void InstallPointOnCar(Vector3 _pointToEnd, float _carMoveSpeed, Transform teleportPoint, Image fade)
    {
        gameObject.GetComponent<TeleportPlayer>().SetInfo(teleportPoint, fade);

        gameObject.GetComponent<BoxCollider>().size *= 1.2f;

        transform.eulerAngles = new Vector3(0, -90, 0);

        var distance = Vector3.Distance(transform.position, _pointToEnd);
        _endTimer = distance / _carMoveSpeed;

        _positionToEnd = _pointToEnd;
        _speed = _carMoveSpeed;

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
        else if (CanBeAffected && TimeManager.instance.IsTimeSlowed && !isTimeSlowed)
        {
            _rigidbody.linearVelocity = (_positionToEnd * _speed / 100) * 0.1f;
            _timer += Time.deltaTime * 0.1f;
            return;
        }
        _rigidbody.linearVelocity = _positionToEnd * _speed / 100;
        _timer += Time.deltaTime;

    }

    public void RealTimeAction()
    {
        _collider.isTrigger = true;

        isTimeStopped = false;
        isTimeSlowed = false;
    }

    public void StopTimeAction()
    {
        _collider.isTrigger = false;
        isTimeStopped = true;
        _rigidbody.linearVelocity = Vector3.zero;
    }

    public void SlowTimeAction()
    {
        _collider.isTrigger = false;
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
