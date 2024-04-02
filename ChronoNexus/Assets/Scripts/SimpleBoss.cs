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
        if( boss == null)
        {
            doortrigger.GetComponent<DoorTrigger>().UnlockDoor();
        }
    }
}
