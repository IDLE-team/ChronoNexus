using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweenBreathing : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
        StartCoroutine(StartThroughTime());
    }

    IEnumerator StartThroughTime()
    {
        yield return new WaitForSeconds(1.5f);
        transform.DOMoveY(transform.position.y + 0.1f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

    }
}
