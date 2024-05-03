using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    [SerializeField] private AudioClip _startMusicBossFight;
    [SerializeField] private AudioClip _middleMusicBossFight;
    [SerializeField] private AudioClip _endMusicBossFight;
    
    [SerializeField] private AudioSource _audioSource;
    
    public static Event OnBossFightStarted;
    public static Event OnBossFightEnded;

    private void StartBossFight()
    {
        
    }

    private void EndBossFight()
    {
        
    }

}
