using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialScreen;
    
    private bool _wasActivated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_wasActivated)
            {
                _tutorialScreen.SetActive(true);
                _tutorialScreen.GetComponent<TutorialController>().StartControllerWork();
            }
        }
    }
    
}
