using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using Random = UnityEngine.Random;

public class HealBuff : MonoBehaviour
{
    private Transform player;
    private Tween followTween;

    [SerializeField] private int _healAmount;
    [SerializeField] private int _maxAmount = 5;
    [SerializeField] private Collider _triggerCollider;


    private void Start()
    {
        StartCoroutine(ActivateTrigger());
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (followTween != null && followTween.IsActive())
        {
            followTween.Kill();
        }

        followTween = transform.DOMove(player.position + new Vector3(0, 1f, 0), 0.1f).SetEase(Ease.InOutSine)
            .OnComplete(
                () => IncreaseHealth());

    }

    private void IncreaseHealth()
    {
        player.GetComponent<Health>().Increase(Random.Range(5, _maxAmount));
        Destroy(gameObject);
    }

    IEnumerator ActivateTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        _triggerCollider.enabled = true;
    }
}
