using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfacePause : MonoBehaviour
{

    private bool _isGame;
    private void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            _isGame = true;
        }
    }

    private void OnEnable()
    {
        if (_isGame)
        {
           // TimeManager.instance.ContinueTime();
        }
    }

    private void OnDisable()
    {
        if (_isGame)
        {
        //    TimeManager.instance.StopTimeInfinite();
        }
    }

}
