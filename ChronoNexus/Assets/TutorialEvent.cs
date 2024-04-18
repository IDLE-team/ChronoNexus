using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialEvent : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialFinisherScreen;
    [SerializeField] private GameObject _tutorialDieScreen;
    [SerializeField] private GameObject _eventHolder;

    [SerializeField] private IFinisherable _finisherable;
    [SerializeField] private Health _damagable;
    private bool _wasActivated;

    private void Start()
    {
        _finisherable = _eventHolder.GetComponent<IFinisherable>();
        _finisherable.OnFinisherReady += FinisherEvent;

        _damagable = _eventHolder.GetComponent<Health>();
        _damagable.Died += DieEvent;
    }

    private void FinisherEvent()
    {
        _tutorialFinisherScreen.SetActive(true);
        _tutorialFinisherScreen.GetComponent<TutorialController>().StartControllerWork();
    }

    private void DieEvent()
    {
        _tutorialDieScreen.SetActive(true);
        _tutorialDieScreen.GetComponent<TutorialController>().StartControllerWork();
    }

}
