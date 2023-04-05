using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Image joystickBG; // ��� �����
    [SerializeField] private Image joystickHandle; // ����� �����
    [SerializeField] private bool isButton; // �������� �� ���� �������
    [SerializeField] private float handleRange = 1; // ������ ����� �����

    private Vector3 inputVector = Vector3.zero; // �������� �����
    private Vector3 defaultPos; // ����������� ������� ����� �����

    private void Start()
    {
        defaultPos = joystickHandle.transform.position; // ���������� ����������� ������� ����� �����
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isButton) // ���� ��� ������, �� ������ �������� ��������������� �������
        {
            // ����� �������� ������� ������� ������
            // ��������, SendMessage("OnButtonPress", SendMessageOptions.DontRequireReceiver);
            return;
        }

        // ���� ��� ����, �� ������ ��� �������� � ������ � ������� �������
        joystickBG.gameObject.SetActive(true);
        joystickBG.transform.position = eventData.position;

        // ������ ����� ����� � ������� ������� � ���������� ��� �������
        joystickHandle.transform.position = eventData.position;
        inputVector = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��� ������, �� ������ �� ������
        if (isButton) return;

        // ��������� ������ �� ����������� ������� �� ������� �������
        Vector3 direction = (eventData.position - new Vector2(joystickBG.transform.position.x, joystickBG.transform.position.y).normalized);

        // ������������ ������ ����� �����, ����� �� �� ������� �� ���
        float distance = Vector3.Distance(eventData.position, joystickBG.transform.position);
        inputVector = distance > handleRange ? direction * handleRange : direction;

        // ���������� ����� ����� �� ����������, ���������������� ������� �����������
        joystickHandle.transform.position = joystickBG.transform.position + inputVector * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ���� ��� ������, �� �������� ��������������� �������
        if (isButton)
        {
            // ����� �������� ������� ���������� ������
            // ��������, SendMessage("OnButtonRelease", SendMessageOptions.DontRequireReceiver);
            return;
        }

        // ���� ��� ����, �� ������ ��� ���������� � ���������� ����� ����� � ����������� �������
        joystickBG.gameObject.SetActive(false);
        joystickHandle.transform.position = defaultPos;
        inputVector = Vector3.zero;
    }

    public Vector3 GetInputVector()
    {
        return inputVector; // ���������� �������� �����
    }
}