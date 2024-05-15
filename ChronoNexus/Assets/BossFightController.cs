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
    
    [SerializeField] private GameObject _blackHole;
    [SerializeField] private GameObject _bossGameObject;
    [SerializeField] private PlayableDirector _timeline;
    
    public static Event OnBossFightStarted;
    public static Event OnBossFightEnded;

    private void StartBossFight()
    {
        
    }

    private void EndBossFight()
    {
        
    }

    public void SetEnableBoss(bool value)
    {
        _bossGameObject.SetActive(value);
    }

    public void SetEnableBlackHole(bool value)
    {
        if (value)
        {
            _blackHole.transform.DOScale(0, 0f);
            _blackHole.SetActive(value);
            _blackHole.transform.DOScale(10f, 2f);
        }
        else
        {
            _blackHole.transform.DOScale(0, 2f).OnComplete(()=>{_blackHole.SetActive(value);});
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _timeline.Play();
        }
    }

}
