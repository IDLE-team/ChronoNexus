using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    static public TimeManager instance;

    public AudioSource audioSource;

    public Volume postProcessVolume;
    public VolumeProfile realTimeVolumeProfile;
    public VolumeProfile timeStopVolumeProfile;

    public Slider debugSlider;
    public TextMeshProUGUI debugText;
    private float basePitch;
    public bool IsTimeStopped;
    public float resumeTimeDelay;
    
    private List<ITimeBody> timeBodies = new List<ITimeBody>();

    private void Start()
    {
        debugSlider.value = resumeTimeDelay;
        debugText.text = debugSlider.value.ToString();
        debugSlider.onValueChanged.AddListener(OnDebugSliderValueChanged);
    }
    public void AddTimeBody(ITimeBody body)
    {
        timeBodies.Add(body);
    }
    public void RemoveTimeBody(ITimeBody body)
    {
        timeBodies.Remove(body);
        Debug.Log("Удалило");
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
    public void ContinueTime()
    {
        IsTimeStopped = false;

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

    private void OnDebugSliderValueChanged(float value)
    {
        resumeTimeDelay = value;
        debugText.text = value.ToString();
    }

    IEnumerator ResumeTimeWithDelay()
    {
        yield return new WaitForSeconds(resumeTimeDelay);
        ContinueTime();
    }
}
