using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    protected Transform player;
    protected Tween followTween;

    [SerializeField] protected int _valueAmount;
    [SerializeField] protected int _maxAmount = 5;
    [SerializeField] protected Collider _triggerCollider;


    protected void Start()
    {
        StartCoroutine(ActivateTrigger());
        Destroy(gameObject, 5f);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            FollowPlayer();
        }
    }

    protected void FollowPlayer()
    {
        if (followTween != null && followTween.IsActive())
        {
            followTween.Kill();
        }

        followTween = transform.DOMove(player.position + new Vector3(0, 1f, 0), 0.1f).SetEase(Ease.InOutSine)
            .OnComplete(
                () => IncreaseValue());

    }

    protected virtual void IncreaseValue()
    {
        Debug.Log("Script: DropObject - Value increased!");
        Destroy(gameObject);
    }

    protected IEnumerator ActivateTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        _triggerCollider.enabled = true;
    }
}
