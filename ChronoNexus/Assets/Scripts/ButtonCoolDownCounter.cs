using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class ButtonCoolDownCounter : MonoBehaviour
{
    [SerializeField] private Image _imageFill;
    [SerializeField] private TextMeshProUGUI _counter;
    [SerializeField] private GameObject _buttonIcon;
    [SerializeField] private GameObject _coolDownHolder;
    [SerializeField] private ICoolDownable _coolDownable;

    private OnScreenButton _button;
    
    private void OnEnable()
    {
        _coolDownable.OnCoolDown += StartCoolDown;
    }

    private void Awake()
    {
        _coolDownHolder.TryGetComponent(out _coolDownable);
        _button = GetComponent<OnScreenButton>();
    }

    public void StartCoolDown(float time)
    {
        StartCoroutine(CoolDown(time));
    }

    IEnumerator CoolDown(float time)
    {
        _button.enabled = false;
        if(_buttonIcon!=null)
            _buttonIcon.SetActive(false);
        _counter.gameObject.SetActive(true);
        _imageFill.gameObject.SetActive(true);
        _imageFill.fillAmount = 1;
        _counter.text = time.ToString();
        _imageFill.DOFillAmount(0, time);
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;
            _counter.text = time.ToString();
        }
        if(_buttonIcon!=null)
            _buttonIcon.SetActive(true);
        _counter.gameObject.SetActive(false);
        _imageFill.gameObject.SetActive(false);
        _button.enabled = true;


    }
    
}
