using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _requiredHoldTime;
    private bool _pointerDown;
    private bool _pointerUp;
    private float _pointerDownTimer;
    
    public UnityEvent onClicked;
    public UnityEvent onLongClicked;

    // [SerializeField]
    // private Image fillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pointerUp = true;
        Debug.Log("OnPointerUp");
    }

    private void Update()
    {
        if (_pointerDown)
        {
            _pointerDownTimer += Time.deltaTime;
            if (_pointerDownTimer >= _requiredHoldTime)
            {
                onLongClicked?.Invoke();
            }
        }
        if (_pointerUp && _pointerDownTimer <= _requiredHoldTime)
        {
            onClicked?.Invoke();
        }
        Reset();
        //    fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
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