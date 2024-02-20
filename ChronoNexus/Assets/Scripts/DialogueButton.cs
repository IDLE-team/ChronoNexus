using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DialogueButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
   [SerializeField] private ProximitySelector _proximitySelector;

    public void OnPointerDown(PointerEventData eventData)
    {
        TaskOnClick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    private void TaskOnClick()
    {
        // ��������� �������, ������� ��������� � ���� Proximity Selector
        _proximitySelector.UseCurrentSelection();
    }
}