using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour , ITimeAffected
{

    public event Action OnTimeAffectedDestroy;
    public bool isTimeStopped { get; set; }
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }
    public void RealTimeAction()
    {
        throw new NotImplementedException();
    }

    public void StopTimeAction()
    {
        throw new NotImplementedException();
    }

    public void SlowTimeAction()
    {
        throw new NotImplementedException();
    }

    public void RewindTimeAction()
    {
        throw new NotImplementedException();
    }

    public void AcceleratedTimeAction()
    {
        throw new NotImplementedException();
    }
}
