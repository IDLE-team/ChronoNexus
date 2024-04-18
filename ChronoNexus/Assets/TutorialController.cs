using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _deactivationObjects;
    [SerializeField] private List<GameObject> _guideScreenElements = new List<GameObject>();
    public void StartControllerWork()
    {
        for (int i = 0; i < _deactivationObjects.Count; i++)
        {
            _deactivationObjects[i].SetActive(false);
        }
    }

    public void DeactivateTutorialScreen()
    {
        for (int i = 0; i < _guideScreenElements.Count; i++)
        {
            _guideScreenElements[i].SetActive(false);
        }
    }
    public void EndControllerWork()
    {
        for (int i = 0; i < _deactivationObjects.Count; i++)
        {
            _deactivationObjects[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
    
}
