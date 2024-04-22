using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveImageBackground : MonoBehaviour
{
    [SerializeField] private RectTransform image;
    [SerializeField] private float time = 5;
    private void Start()
    {
        StartCoroutine(MoveImage());
    }

    IEnumerator MoveImage()
    {
        image.anchoredPosition = Vector2.zero;
        image.DOAnchorPosX( - image.rect.width, time).SetEase(Ease.Linear);
        yield return new WaitForSeconds(time);
        StartCoroutine(MoveImage());
    }
}
