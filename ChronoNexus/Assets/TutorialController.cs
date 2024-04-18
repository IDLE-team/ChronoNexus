using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _deactivationObjects;
    public void StartControllerWork()
    {
        for (int i = 0; i < _deactivationObjects.Count; i++)
        {
            _deactivationObjects[i].SetActive(false);
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
