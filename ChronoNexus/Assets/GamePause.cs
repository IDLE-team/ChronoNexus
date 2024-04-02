using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{

    private TimeManager _manager;

    private void Start()
    {
        _manager = FindFirstObjectByType<TimeManager>();
    }

    public void PauseGame()
    {
        if (_manager)
        {
            _manager.StopTimeInfinite();
        }
    }

    public void ResumeGame()
    {
        if (_manager)
        {
            _manager.ContinueTime();
        }
    }
}
