using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevindedObjectMove : MonoBehaviour , ITimeAffected
{
    [SerializeField] private Transform _movePosition;
    [SerializeField] private Transform _middlePostiton;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private float _time = 1f;
    [SerializeField] private GameObject _object;
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get ; set ; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }

    public event Action OnTimeAffectedDestroy;

    private Tween _tween;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            MoveObject();
        }
    }
    public void MoveObject()
    {
        _tween = _object.transform.DOPath(new Vector3[] { _startPosition.position, _middlePostiton.position, _movePosition.position }, _time);
    }
    public void AcceleratedTimeAction()
    {
        
    }

    public void RealTimeAction()
    {
        _tween.Play();
    }

    public void RewindTimeAction()
    {
        Debug.Log(this.gameObject + " RewindTimeAction() ");
        _tween = _object.transform.DOPath(new Vector3[] { _movePosition.position, _middlePostiton.position, _startPosition.position }, _time);
        //_tween.Play();
    }

    public void SlowTimeAction()
    {
        
    }

    public void StopTimeAction()
    {
        _tween.Pause();
    }


}
