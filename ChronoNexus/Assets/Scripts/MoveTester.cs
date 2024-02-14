using System;
using UnityEngine;

public class MoveTester : MonoBehaviour, ITimeAffected
{

    public float baseSpeed = 5f;
    public float speed = 5f; // Скорость движения объекта

    public event Action OnTimeAffectedDestroy;

    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    public void AcceleratedTimeAction()
    {
        isTimeAccelerated = true;
    }

    public void RealTimeAction()
    {
        isTimeStopped = false;
        isTimeAccelerated = false;
        isTimeSlowed = false;
        isTimeRewinded = false;
        speed = baseSpeed;
    }

    public void RewindTimeAction()
    {
        isTimeRewinded = true;
    }

    public void SlowTimeAction()
    {
        isTimeSlowed = true;
    }

    public void StopTimeAction()
    {
        isTimeStopped = true;
        speed = 0.0f;
    }

    private void Start()
    {
        speed = baseSpeed;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float newPositionX;
            newPositionX = currentPosition.x + Mathf.Sin(Time.time) * speed * Time.deltaTime;
            transform.position = new Vector3(newPositionX, currentPosition.y, currentPosition.z);


    }
    private void OnDestroy()
    {
        OnTimeAffectedDestroy?.Invoke();

    }
}
