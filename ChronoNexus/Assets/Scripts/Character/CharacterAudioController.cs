using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudioController : MonoBehaviour
{
    [Header("Player SFX")]
    [Tooltip("Footstep SFX")]
    [SerializeField] private List<AudioClip> _footStepClips;

    [Tooltip("HIT SFX")]
    [SerializeField] private List<AudioClip> _hitClips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstepSound()
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