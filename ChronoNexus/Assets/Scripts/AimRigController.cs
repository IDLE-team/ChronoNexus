using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG;
using UnityEngine.SocialPlatforms.Impl;
public class AimRigController : MonoBehaviour
{
    [SerializeField] private List<Rig> _rigList = new List<Rig>();
    [SerializeField] private float _smoothTime;
    private Rig _currentRig;
    public Rig CurrentRig => _currentRig;
    private float prevWeight;
    public Transform _aimTarget;
    private IEnumerator coroutine;
    [SerializeField] private List<MultiAimConstraint> _constraints = new List<MultiAimConstraint>();


    private void Start()
    {
        _currentRig = _rigList[0];
    }

    public void SetWeight(float weight)
    {
        _currentRig.weight = weight;
    }

    public void SetSmoothWeight(float weight)
    {
        if (coroutine != null)
        {
            StopSmoothWeight();
        }
        coroutine = SmootherWeight(weight);
        StartCoroutine(coroutine);
    }

    public void StopSmoothWeight()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
    }
    public void SetCurrentRig(int rigID)
    {
        if(_currentRig == null)
            return;
        if(_currentRig == _rigList[rigID])
            return;
        float _previousRigWeight = _currentRig.weight;
        SetWeight(0);
        _currentRig = _rigList[rigID];
        SetWeight(_previousRigWeight);
    }

    IEnumerator SmootherWeight(float weight)
    {
      //  bool _smooth = true;
        float elapsedTime = 0;
        while (elapsedTime < _smoothTime)
        {
            _currentRig.weight = Mathf.Lerp(_currentRig.weight, weight, (elapsedTime / _smoothTime));
            elapsedTime += Time.deltaTime;
            
            if (Mathf.Abs(_currentRig.weight - weight) <= 0.01)
            {
                _currentRig.weight = weight;
                yield return null;
            }
            
            yield return new WaitForEndOfFrame();

        }

        yield return null;
    }
    
}
