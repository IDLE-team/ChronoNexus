using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GradientMover : MonoBehaviour
{
    [SerializeField] private float _delay = 1;
    [SerializeField] private float _secondsToCross = 3;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private GameObject _gradient;

    private void Start()
    {
        StartCoroutine(MoveGradient());
    }

    IEnumerator MoveGradient()
    {
        _gradient.transform.position = _startPoint.position;
        _gradient.transform.DOMove(_endPoint.position, _secondsToCross);
        yield return new WaitForSeconds(_secondsToCross+_delay);

        StartCoroutine(MoveGradient());
    }
}
