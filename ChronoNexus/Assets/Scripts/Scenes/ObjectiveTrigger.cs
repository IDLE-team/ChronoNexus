using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour, SceneObjective
{
    public GameObject disableObject;
    public void Activate()
    {
        disableObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Activate();
        }
    }  
}
