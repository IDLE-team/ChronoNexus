using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Layouts;

namespace UnityEngine.InputSystem.OnScreen
{
    public class HoverJoystick : Joystick
    {
        public UnityEvent onPointerUp;
        private PointerEventData pointerEventData;
        private DefaultInputActions inputActions;
        public InputActionReference rightClick { get; set; }
        protected override void Start()
        {
            base.Start();
           // background.gameObject.SetActive(false);
        }

        //public void Initialize(PointerEventData eventData)
        //{
            //  Debug.Log(eventData);
            //   pointerEventData = eventData;
           //eventData.pointerDrag = gameObject;
           //eventData.pointerPress = gameObject;
            //pointerEventData.Reset();

            // ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerDownHandler);
            //OnPointerDown(eventData);
            //SendValueToControl(eventData.[]);
            //   eventData.pointerClick = gameObject;
            // eventData.dragging = true;
            //  eventData.Use();
            // eventData.selectedObject = gameObject;
            //eventData.pointerPress = gameObject;

            //PointerEventData eventDataNew = new PointerEventData(EventSystem.current);

            // OnPointerDown(eventData);

        //}

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            onPointerUp?.Invoke();
        }
    }
}