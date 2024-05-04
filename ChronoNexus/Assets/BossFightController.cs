using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class BossFightController : MonoBehaviour
{
    [SerializeField] private AudioClip _startMusicBossFight;
    [SerializeField] private AudioClip _middleMusicBossFight;
    [SerializeField] private AudioClip _endMusicBossFight;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayableDirector _startTimeLine;
    [SerializeField] private GameObject _boss;
    [SerializeField] private GameObject _blackHole;
    
    private Collider trigger;
    
    public static Event OnBossFightStarted;
    public static Event OnBossFightEnded;

    private void Awake()
    {
        trigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartBossFight();
        }
    }

    private void StartBossFight()
    {
        _startTimeLine.Play();
        
    }

    public void SetActiveBoss(bool value)
    {
        _boss.SetActive(value);
    }
    public void SetActiveBlackHole(bool value)
    {
        if (value)
        {
            _blackHole.transform.DOScale(0, 0);
            _blackHole.SetActive(true);
            _blackHole.transform.DOScale(10f, 1f);
        }
        else
        {
            _blackHole.transform.DOScale(10, 0);
            _blackHole.SetActive(true);
            _blackHole.transform.DOScale(0, 1f).OnComplete(() => _blackHole.SetActive(value));
        }
    }
    
    
    
    

    private void EndBossFight()
    {
        
    }

    public void PlayMusic()
    {
        _audioSource.Play();
    }

}
