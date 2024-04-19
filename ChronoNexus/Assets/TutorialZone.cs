using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialScreen;
    [SerializeField] private List<GameObject> _objectsToAppear = new List<GameObject>();
    private bool _wasActivated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_wasActivated)
            {
                _tutorialScreen.SetActive(true);
                _tutorialScreen.GetComponent<TutorialController>().StartControllerWork();
                _wasActivated = true;
                if (_objectsToAppear.Count > 0)
                {
                    for (int i = 0; i < _objectsToAppear.Count; i++)
                    {
                        _objectsToAppear[i].SetActive(true);
                    }
                }
            }
        }
    }
    
}
