using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private bool pointerUp;

    private float pointerDownTimer;

    [SerializeField]
    private float requiredHoldTime;

    public UnityEvent onClick;
    public UnityEvent onLongClick;

    // [SerializeField]
    // private Image fillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerUp = true;
        Debug.Log("OnPointerUp");
    }

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                if (onLongClick != null)
                    onLongClick.Invoke();
            }
        }
        if (pointerUp && pointerDownTimer <= requiredHoldTime)
        {
            if (onClick != null)
                onClick.Invoke();
            Reset();
        }
        else if (pointerUp && pointerDownTimer >= requiredHoldTime)
        {
            Reset();
        }
        //    fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
    }

    private void Reset()
    {
        pointerDown = false;
        pointerUp = false;
        pointerDownTimer = 0;
        //  fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
    }
}