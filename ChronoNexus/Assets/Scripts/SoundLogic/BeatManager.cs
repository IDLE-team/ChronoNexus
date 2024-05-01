using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] public float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] public Intervals[] _intervals;
    [SerializeField] public float sampledtime;

    

    private void Update()
    {
        if(_audioSource.clip == null)
            return;
        foreach (Intervals interval in _intervals)
        {
            sampledtime = (_audioSource.timeSamples /
                                 (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForNewInterval(sampledtime);

        }
    }

    [System.Serializable]
    public class Intervals
    {
        [SerializeField] private float _steps;
        [SerializeField] private UnityEvent _trigger;
        private int _lastInterval;

        public float GetIntervalLength(float bpm)
        {
            return 60f / (bpm * _steps);
        }

        public void CheckForNewInterval(float interval)
        {
            if (Mathf.FloorToInt(interval) != _lastInterval)
            {
                _lastInterval = Mathf.FloorToInt(interval);
                _trigger.Invoke();
            }
        }
    }
}
