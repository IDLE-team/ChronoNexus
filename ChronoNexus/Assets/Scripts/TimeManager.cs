using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

public class TimeManager : MonoBehaviour
{

    static public TimeManager instance;

    public AudioSource audioSource;

    public Volume postProcessVolume;
    public VolumeProfile realTimeVolumeProfile;
    public VolumeProfile timeStopVolumeProfile;

    private float basePitch;
    public bool IsTimeStopped;
    public bool IsTimeSlowed;

    public float resumeTimeDelay;

    private PlayerInputActions _input;
    private float audioVolume;

    [Inject]
    private void Construct(PlayerInputActions input)
    {
        _input = input;
        _input.Player.TimeStop.performed += OnStopTimePerformed;
        _input.Player.TimeSlow.performed += OnSlowTimePerformed;

    }

    private List<ITimeBody> timeBodies = new List<ITimeBody>();

    private void OnStopTimePerformed(InputAction.CallbackContext obj)
    {
        if(IsTimeStopped || IsTimeSlowed)
            return;
        StopTime();
    }
    private void OnSlowTimePerformed(InputAction.CallbackContext obj)
    {
        if(IsTimeStopped || IsTimeSlowed)
            return;

        SlowTime();
    }

    public void AddTimeBody(ITimeBody body)
    {
        timeBodies.Add(body);
    }
    public void RemoveTimeBody(ITimeBody body)
    {
        timeBodies.Remove(body);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioVolume = audioSource.volume;
    }
    public void ContinueTime()
    {
        IsTimeStopped = false;
        IsTimeSlowed = false;
        for (var i = 0; i < timeBodies.Count; i++)
        {
            if (timeBodies[i] == null)
            {
                timeBodies.RemoveAt(i);
                continue;
            }
            timeBodies[i].SetRealTime();
        }
        audioSource.pitch = basePitch;

        postProcessVolume.profile = realTimeVolumeProfile;
        audioSource.volume = audioVolume;
    }
    public void StopTime()
    {
        IsTimeStopped = true;
        for (var i = 0; i < timeBodies.Count; i++)
        {
            if (timeBodies[i] == null)
            {
                timeBodies.RemoveAt(i);
                continue;
            }
            timeBodies[i].SetStopTime();
        }
        postProcessVolume.profile = timeStopVolumeProfile;
        basePitch = audioSource.pitch;
        audioSource.pitch = 0.3f;
        
        StartCoroutine(ResumeTimeWithDelay());
    }

    public void StopTimeInfinite() 
    {
        IsTimeStopped = true;
        for (var i = 0; i < timeBodies.Count; i++)
        {
            if (timeBodies[i] == null)
            {
                timeBodies.RemoveAt(i);
                continue;
            }
            timeBodies[i].SetStopTime();
        }
        postProcessVolume.profile = timeStopVolumeProfile;
        basePitch = audioSource.pitch;
        audioSource.pitch = 0.3f;
        audioSource.volume = 0;
    }
    public void SlowTime()
    {
        IsTimeSlowed = true;
        for (var i = 0; i < timeBodies.Count; i++)
        {
            if (timeBodies[i] == null)
            {
                timeBodies.RemoveAt(i);
                continue;
            }
            Debug.Log(timeBodies[i]);
            timeBodies[i].SetSlowTime();
        }
        postProcessVolume.profile = timeStopVolumeProfile;
        basePitch = audioSource.pitch;
        audioSource.pitch = 0.5f;
        StartCoroutine(ResumeTimeWithDelay());
    }
    private void OnDebugSliderValueChanged(float value)
    {
        resumeTimeDelay = value;
    }

    IEnumerator ResumeTimeWithDelay()
    {
        yield return new WaitForSeconds(resumeTimeDelay);
        ContinueTime();
    }
}
