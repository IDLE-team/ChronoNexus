using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class FoundEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    private CancellationTokenSource cancellationTokenSource;
    
    private void OnEnable()
    {
        _effect.Play();
        cancellationTokenSource = new CancellationTokenSource();
        Disable(cancellationTokenSource.Token).Forget();
    }

    private void Update()
    {
        //gameObject.transform.LookAt(Camera.main.transform.position);
    }

    private async UniTask Disable(CancellationToken cancellationToken)
    {
        await UniTask.Delay((int) 2 * 1000);
        gameObject.SetActive(false);
        await UniTask.Yield();
    }
}
