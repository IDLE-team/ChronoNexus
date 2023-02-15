using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, ITargetable
{
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private float _health;

    private ParticleSystem _hitEffect;

    private AudioSource _audioSource;

    private Animator _animator;

    private bool _isAlive;
    private bool _targeted;

    private void Start()
    {
        _hitEffect = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _isAlive = true;
    }

    public void TakeDamage(int damage)
    {
        if (_isAlive)
        {
            _health -= damage;
            DamageEffect();
            _animator.SetTrigger("TakeHit");

            if (_health < 0)
            {
                Death();
            }
        }
    }

    public void Death()
    {
        _health = 0;
        _isAlive = false;
        Destroy(gameObject);
    }

    private void DamageEffect()
    {
        _audioSource.PlayOneShot(_hitClip);
        _hitEffect.Play();
    }

    public void ToggleSelfTarget()
    {
        _targeted = !_targeted;
        transform.GetChild(0).gameObject.SetActive(_targeted);
    }
}