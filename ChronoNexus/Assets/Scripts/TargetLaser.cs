using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLaser : MonoBehaviour
{
    [SerializeField] private LineRenderer _laserRenderer;
    [SerializeField]private Transform _laserOrigin;
    private MovableSoldierEntity _entity;
    private TargetFinder _targetFinder;
    private IEnumerator _coroutine;
    private void Awake()
    {
        _entity= GetComponent<MovableSoldierEntity>();
        _targetFinder = GetComponent<TargetFinder>();
        _targetFinder.OnTargetFinded += StartTargeting;
        _entity.OnDie += StopTargeting; 
        
    }

    private void OnDisable()
    {
        _targetFinder.OnTargetFinded -= StartTargeting;
    }

    private void StartTargeting(ITargetable target)
    {
        if (_coroutine!=null)
        {
            StopCoroutine(_coroutine);
        }
        StartCoroutine(_coroutine = TargetingCoroutine(target));
    }

    private void StopTargeting()
    {
        StopCoroutine(_coroutine); 
        _laserRenderer.gameObject.SetActive(false);
    }

    IEnumerator TargetingCoroutine(ITargetable target)
    {
        
        while (target != null)
        { 
            _laserRenderer.SetPosition(0,_laserOrigin.position);
            _laserRenderer.SetPosition(1,target.GetTransform().position);
            yield return new WaitForSeconds(0.005f);
        }
        yield return null;
    }
}
