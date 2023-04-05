using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image joystickBG; // фон стика
    [SerializeField] private Image joystickHandle; // ручка стика
    [SerializeField] private bool isButton; // является ли стик кнопкой
    [SerializeField] private float handleRange = 1; // радиус ручки стика

    private Vector3 inputVector = Vector3.zero; // значение стика
    private Vector3 defaultPos; // стандартная позиция ручки стика

    private void Start()
    {
        defaultPos = joystickHandle.transform.position; // запоминаем стандартную позицию ручки стика
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isButton) // если это кнопка, то просто вызываем соответствующее событие
        {
            // здесь вызываем событие зажатия кнопки
            // например, SendMessage("OnButtonPress", SendMessageOptions.DontRequireReceiver);
            return;
        }

        // если это стик, то делаем его активным и ставим в позицию касания
        joystickBG.gameObject.SetActive(true);
        joystickBG.transform.position = eventData.position;

        // ставим ручку стика в позицию касания и запоминаем эту позицию
        joystickHandle.transform.position = eventData.position;
        inputVector = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // если это кнопка, то ничего не делаем
        if (isButton) return;

        // вычисляем вектор от стандартной позиции до позиции касания
        Vector3 direction = (eventData.position - new Vector2(joystickBG.transform.position.x, joystickBG.transform.position.y).normalized);

        // ограничиваем радиус ручки стика, чтобы он не выходил за фон
        float distance = Vector3.Distance(eventData.position, joystickBG.transform.position);
        inputVector = distance > handleRange ? direction * handleRange : direction;

        // перемещаем ручку стика на расстояние, пропорциональное радиусу ограничения
        joystickHandle.transform.position = joystickBG.transform.position + inputVector * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // если это кнопка, то вызываем соответствующее событие
        if (isButton)
        {
            // здесь вызываем событие отпускания кнопки
            // например, SendMessage("OnButtonRelease", SendMessageOptions.DontRequireReceiver);
            return;
        }

        // если это стик, то делаем его неактивным и возвращаем ручку стика в стандартную позицию
        joystickBG.gameObject.SetActive(false);
        joystickHandle.transform.position = defaultPos;
        inputVector = Vector3.zero;
    }

    public Vector3 GetInputVector()
    {
        return inputVector; // возвращаем значение стика
    }
}