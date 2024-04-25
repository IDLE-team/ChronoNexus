using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetFollow : MonoBehaviour, ITimeAffected
{
    public Transform target; // Цель, за которой следует объект
    private Tweener followTween; // Tween для анимации следования
    [SerializeField]
    private float followSpeed = 0.1f; // Начальная скорость следования

    public event Action OnTimeAffectedDestroy;
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    private bool _isStopped;
    void Start()
    {
        followTween = transform.DOMove(target.position, followSpeed).SetAutoKill(false);
    }

    void Update()
    {
        
        if (_isStopped)
           return;

        if (!followTween.IsPlaying())
           followTween.Play();
       if(gameObject != null && target != null)
         followTween.ChangeEndValue(target.position,  followSpeed, true);
    }
    public void RealTimeAction()
    {
        followSpeed = 0.1f;
        _isStopped = false;

        followTween.Play();
    }

    public void StopTimeAction()
    {
        followTween.Pause();
        _isStopped = true;
    }

    public void SlowTimeAction()
    {
        followSpeed = 2.2f;
    }

    public void RewindTimeAction()
    {
        throw new NotImplementedException();
    }

    public void AcceleratedTimeAction()
    {
        throw new NotImplementedException();
    }
}
