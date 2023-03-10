using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
    [Header("Player SFX")]
    [Tooltip("Footstep SFX")]
    [SerializeField] private List<AudioClip> _footStepClips = new List<AudioClip>();

    [Tooltip("HIT SFX")]
    [SerializeField] private List<AudioClip> _hitClips = new List<AudioClip>();

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFoostepSound()
    {
        _audioSource.clip = _footStepClips[Random.Range(0, _footStepClips.Count)];
        _audioSource.Play();
    }

    public void PlayHitSound()
    {
        _audioSource.clip = _hitClips[Random.Range(0, _hitClips.Count)];
        _audioSource.Play();
    }
}