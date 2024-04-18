using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerDown : MonoBehaviour, IPointerMoveHandler
{
    [SerializeField] private TutorialController _tutorialController;
    public UnityEvent _pointerDownAction;
    

    public void OnPointerMove(PointerEventData eventData)
    {
        _tutorialController.DeactivateTutorialScreen();
    }
}
