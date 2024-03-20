using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class AnimationEventsHolder : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private Character _character;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource ;

    [SerializeField] private float _smoothTime;
    [SerializeField] private float _finisherOrthographicSize;
    [SerializeField] private float _finisherVignetteIntensity;

    [SerializeField] private GameObject _ui;

    [SerializeField] private Volume _volume;
    
    private float _startOrthographicSize;
    private float _startVignetteIntensity;

    private Vignette _vignette;
    
    private void Start()
    {
        _volume.profile.TryGet(out _vignette);
    }

    public void WeaponFire()
    {
        _weaponController.CurrentWeapon.Fire(_character.CharacterTargetingSystem.Target, _character.Transform);
    }

    public void WeaponAreaFire()
    {
        _weaponController.CurrentWeapon.AreaFire(_character.CharacterTargetingSystem.TargetLayer);
    }

    public void StartFinisher()
    {
        ColdWeapon finisherWeapon = _weaponController.CurrentWeapon as ColdWeapon;
      //  if (finisherWeapon.Distance < Vector3.Distance(transform.position, _character.CharacterTargetingSystem.Target.GetTransform().position))
         //   return;
         _startOrthographicSize = _virtualCamera.m_Lens.OrthographicSize;
         _ui.SetActive(false);
         _startVignetteIntensity = _vignette.intensity.value;
         Debug.Log("_startVignetteIntensity: " + _startVignetteIntensity);
         _vignette.intensity.value = _finisherVignetteIntensity;
        WeaponAreaFire();
        StartCoroutine(SmootherVignette(_finisherVignetteIntensity));
        StartCoroutine(Smoother(_finisherOrthographicSize));
        _character.Movement.LockMove();
        _character.AimRigController.SetWeight(0);
        Vector3 dir = _character.CharacterTargetingSystem.Target.GetTransform().position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        Vector3 newPosition = _character.CharacterTargetingSystem.Target.GetTransform().position + _character.CharacterTargetingSystem.Target.GetTransform().forward * -0.9f;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    public void ScreenShake()
    {
        _cinemachineImpulseSource.GenerateImpulse();
    }
    public void EndFinisher()
    {
        _ui.SetActive(true);
        _vignette.intensity.value = _startVignetteIntensity;
        StartCoroutine(SmootherVignette(_startVignetteIntensity));

        StartCoroutine(Smoother(_startOrthographicSize));
        _character.Movement.UnlockMove();
        _character.AimRigController.SetWeight(1);
    }
    
    IEnumerator Smoother(float lastValue)
    {
        float elapsedTime = 0;
        while (elapsedTime < _smoothTime)
        {
            _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(  _virtualCamera.m_Lens.OrthographicSize, lastValue, (elapsedTime / _smoothTime));
            elapsedTime += Time.deltaTime;
            
            if (Mathf.Abs( _virtualCamera.m_Lens.OrthographicSize - lastValue) <= 0.01)
            {
                _virtualCamera.m_Lens.OrthographicSize = lastValue;
                yield return null;
            }
            
            yield return new WaitForEndOfFrame();

        }

        yield return null;
    }
    IEnumerator SmootherVignette(float lastValue)
    {
        float elapsedTime = 0;
        while (elapsedTime < _smoothTime)
        {
            _vignette.intensity.value = Mathf.Lerp(     _vignette.intensity.value, lastValue, (elapsedTime / _smoothTime));
            elapsedTime += Time.deltaTime;
            
            if (Mathf.Abs(    _vignette.intensity.value - lastValue) <= 0.01)
            {
                _vignette.intensity.value = lastValue;
                yield return null;
            }
            
            yield return new WaitForEndOfFrame();

        }

        yield return null;
    }
}
