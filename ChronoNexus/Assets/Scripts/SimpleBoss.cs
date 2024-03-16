using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoss : MonoBehaviour
{
    [SerializeField] private GameObject boss;

    [SerializeField] private GameObject doortrigger;
    private bool _doorWasOpen;
    private void Update()
    {
        if(boss== null && !_doorWasOpen)
        {
            doortrigger.GetComponent<DoorTrigger>().UnlockDoor();
            _doorWasOpen = true;    
        }
    }
}
