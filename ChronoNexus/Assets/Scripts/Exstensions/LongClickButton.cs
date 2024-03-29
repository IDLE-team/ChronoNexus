using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem.Layouts;
namespace UnityEngine.InputSystem.OnScreen
{
    public class LongClickButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _requiredHoldTime;

        private bool _pointerDown;
        private bool _pointerUp;
        private float _pointerDownTimer;

        public event Action OnClicked;

        public event Action OnLongClicked;

        public PointerEventData PointerEventData { get; private set; }

        [InputControl(layout = "Button")]
        [SerializeField]
        private string m_ControlPath;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerEventData = eventData;
            _pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerDownTimer < _requiredHoldTime)
            {
                //OnClicked?.Invoke();
                SendValueToControl(1.0f);
                SendValueToControl(0.0f);
            }
            Reset();
        }


        private void Update()
        {
            if (_pointerDown)
            {
                _pointerDownTimer += Time.deltaTime;
                if (_pointerDownTimer >= _requiredHoldTime)
                {
                    OnLongClicked?.Invoke();
                    _pointerDown = false;
                }
            }
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
}