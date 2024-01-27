using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LongClickButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float _requiredHoldTime;

    private bool _pointerDown;
    private bool _pointerUp;
    private float _pointerDownTimer;

    public event Action OnClicked;

    public event Action OnLongClicked;

    public PointerEventData PointerEventData { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer: " + PointerEventData);
        PointerEventData = eventData;
        _pointerDown = true;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _pointerDown = false;
        _pointerUp = true;
    }

    private void Update()
    {
        if (_pointerDown)
        {
            _pointerDownTimer += Time.deltaTime;
            if (_pointerDownTimer >= _requiredHoldTime)
            {
                OnLongClicked?.Invoke();
                Reset();
            }
        }
        if (!_pointerUp || !(_pointerDownTimer <= _requiredHoldTime))
            return;
        OnClicked?.Invoke();
        Reset();
    }

    [Fix]
    private void Reset()
    {
        _pointerDown = false;
        _pointerUp = false;
        _pointerDownTimer = 0;
        //  fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
    }
}