using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceActivator : MonoBehaviour
{
    [SerializeField] private AudioClip _voiceClip;
    [SerializeField] private AudioSource _voiceSource;
    
    private bool _wasActivated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_wasActivated)
            {
                _voiceSource.clip = _voiceClip;
                _voiceSource.Play();
                _wasActivated = true;
            }
        }
    }
}
