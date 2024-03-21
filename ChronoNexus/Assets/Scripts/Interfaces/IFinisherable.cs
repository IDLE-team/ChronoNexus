using System;

public interface IFinisherable
{
    public void StartFinisher();
    public bool GetFinisherableStatus();

    public event Action OnFinisherReady;
    public event Action OnFinisherEnded;

}