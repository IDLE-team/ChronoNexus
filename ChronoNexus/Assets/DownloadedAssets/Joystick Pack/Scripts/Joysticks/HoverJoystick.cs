﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverJoystick : Joystick
{
    public UnityEvent onPointerUp;
    private PointerEventData pointerEventData;

    public void Initialize(PointerEventData eventData)
    {
        eventData.pointerDrag = gameObject;
        eventData.pointerPress = gameObject;
        OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointerUp?.Invoke();
    }
}