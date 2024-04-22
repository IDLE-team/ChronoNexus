using System;

public interface IFinisherable
{
    public void StartFinisher(int id);
    public bool GetFinisherableStatus();

    public event Action OnFinisherReady;
    public event Action OnFinisherEnded;

    public event Action OnFinisherInvalid;

}