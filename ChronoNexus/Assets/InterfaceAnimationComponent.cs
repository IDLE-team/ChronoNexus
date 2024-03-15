using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(UserInterfaceManager))]
public class InterfaceAnimationComponent : MonoBehaviour
{
    // Off Screen
    public void MoveDownOffScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(0, 0, 0);
        rect.DOLocalMoveY(-Screen.height, duration);
    }
    public void MoveUpOffScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(0, 0, 0);
        rect.DOLocalMoveY(Screen.height, duration);
    }

    public void MoveRightOffScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(0, 0, 0);
        rect.DOLocalMoveX(Screen.width, duration);
    }

    public void MoveLeftOffScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(0, 0, 0);
        rect.DOLocalMoveX(-Screen.width, duration);
    }

    // On Screen
    public void MoveUpOnScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(0, -Screen.height, 0);
        rect.DOLocalMoveY(0, duration);
    }

    public void MoveDownOnScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(0, Screen.height, 0);
        rect.DOLocalMoveY(0, duration);
    }

    public void MoveLeftOnScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(Screen.width, 0, 0);
        rect.DOLocalMoveX(0, duration);
    }

    public void MoveRightOnScreen(RectTransform rect, float duration)
    {
        rect.localPosition.Set(-Screen.width, 0, 0);
        rect.DOLocalMoveX(0, duration);
    }
}
