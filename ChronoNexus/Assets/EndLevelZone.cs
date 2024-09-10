using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.Win();
        }
    }
}
