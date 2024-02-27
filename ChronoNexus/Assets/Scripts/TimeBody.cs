using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ITimeAffected))]
public class TimeBody : MonoBehaviour, ITimeBody
{
    private ITimeAffected _timeAffectedBody;

    private void OnEnable()
    {
        _timeAffectedBody.OnTimeAffectedDestroy += RemoveFromTimeManager;

        if (TimeManager.instance != null)
        {
            AddToTimeManager();
        }
        else
        {
            StartCoroutine(TimeManagerWaiter());
        }
    }
    private void OnDisable()
    {
        _timeAffectedBody.OnTimeAffectedDestroy -= RemoveFromTimeManager;
    }

    private IEnumerator TimeManagerWaiter()
    {
        yield return new WaitUntil(() => TimeManager.instance != null);
        AddToTimeManager();
    }

    void Awake()
    {
        _timeAffectedBody = GetComponent<ITimeAffected>();
    }

    public void SetAcceleratedTime()
    {
        _timeAffectedBody.AcceleratedTimeAction();
    }

    public void SetRealTime()
    {
        _timeAffectedBody.RealTimeAction();
    }

    public void SetRewindTime()
    {
        _timeAffectedBody.RewindTimeAction();

    }

    public void SetSlowTime()
    {
        _timeAffectedBody.SlowTimeAction();
    }

    public void SetStopTime()
    {
        _timeAffectedBody.StopTimeAction();
    }

    public void AddToTimeManager()
    {
        TimeManager.instance.AddTimeBody(this);
    }
    public void RemoveFromTimeManager()
    {
        TimeManager.instance.RemoveTimeBody(this);
    }
}
