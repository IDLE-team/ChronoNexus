using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

public class TimeManager : MonoBehaviour, ICoolDownable
{

    [SerializeField] private float _stoptimeCooldown;
    
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

    [SerializeField] private GameObject _timeManipulationCam;
    
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
        _timeManipulationCam.SetActive(false);

        audioSource.volume = audioVolume;
    }

   public void ContinueTimeForSingle(ITimeBody timeBody)
    {
        timeBody.SetRealTime();
    }
    public void StopTime()
    {
        if(IsTimeStopped)
            return;
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
        _timeManipulationCam.SetActive(true);
        postProcessVolume.profile = timeStopVolumeProfile;
        basePitch = audioSource.pitch;
        audioSource.pitch = 0.3f;
        OnCoolDown?.Invoke(_stoptimeCooldown);
        StartCoroutine(ResumeTimeWithDelay());
    }
    public void StopTimeForAllExcept(List<ITimeBody> nonStopTimeBody, float durationTime)
    {
        if (IsTimeStopped)
        {
            ContinueTimeForSingle(nonStopTimeBody[0]);
            return;
        }

        IsTimeStopped = true;
        List<ITimeBody> filteredTimeBodiesList = timeBodies.FindAll(i => !nonStopTimeBody.Contains(i));
        for (var i = 0; i < filteredTimeBodiesList.Count; i++)
        {
            if (filteredTimeBodiesList[i] == null)
            {
                filteredTimeBodiesList.RemoveAt(i);
                continue;
            }
            filteredTimeBodiesList[i].SetStopTime();
        }
        _timeManipulationCam.SetActive(true);

        postProcessVolume.profile = timeStopVolumeProfile;
        basePitch = audioSource.pitch;
        audioSource.pitch = 0.3f;
        if(durationTime < 0)
            return;
        StartCoroutine(ResumeTimeWithDelay(durationTime));

    }
    public void StopTimeInfinite() 
    {       
        if(IsTimeStopped)
            return;
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
        _timeManipulationCam.SetActive(true);

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
    IEnumerator ResumeTimeWithDelay(float resumeDelay)
    {
        yield return new WaitForSeconds(resumeDelay);
        ContinueTime();
    }

    public event Action<float> OnCoolDown;
}
