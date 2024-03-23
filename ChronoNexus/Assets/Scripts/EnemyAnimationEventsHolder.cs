using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventsHolder : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private EnemyHumanoid _enemy;
    [SerializeField] private ParticleSystem _finisherVFX;
    [SerializeField] private ParticleSystem _finisherFinalVFX;

    public void WeaponFire()
    {
        _weaponController.CurrentWeapon.Fire(_enemy.TargetFinder.Target, _enemy.transform);
    }

    public void PlayFinisherVFX()
    {
        _finisherVFX.Play();
        _finisherVFX.GetComponent<AudioSource>().Play();
    }
    public void PlayFinalFinisherVFX()
    {
        _finisherFinalVFX.Play();
        _finisherFinalVFX.GetComponent<AudioSource>().Play();

    }

    
    
}
