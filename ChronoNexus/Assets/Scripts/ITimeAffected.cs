using System;
public interface ITimeAffected
{
    public event Action OnTimeAffectedDestroy;
    public bool isTimeStopped { get; set;}
    public bool isTimeAccelerated { get; set; }
    public bool isTimeSlowed { get; set; }
    public bool isTimeRewinded { get; set; }
    public void RealTimeAction();
    public void StopTimeAction();
    public void SlowTimeAction();
    public void RewindTimeAction();
    public void AcceleratedTimeAction();
}
