using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace UnityEngine.InputSystem.OnScreen
{
    public class HoverJoystick : Joystick
    {
        public UnityEvent onPointerUp;
        private PointerEventData pointerEventData;

        protected override void Start()
        {
            base.Start();
            background.gameObject.SetActive(false);
        }

        public void Initialize(PointerEventData eventData)
        {
            Debug.Log(eventData);
            eventData.pointerDrag = gameObject;
            eventData.pointerPress = gameObject;
            eventData.Reset();
    OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("PointerUp");
            base.OnPointerUp(eventData);
            onPointerUp?.Invoke();
        }
    }
}