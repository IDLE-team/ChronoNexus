using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value;
    [SerializeField] private TMP_InputField _healthSetter;
    [SerializeField] private ParticleSystem _healEffect;
    private float _maxHealth;
    public float MaxHealth => _maxHealth;
    public float Value => _value;

    public event Action Died;

    public event Action<float> Changed;

    private bool _isInvinsible;

    public bool IsInvincible => _isInvinsible;
    
    private void OnEnable()
    {
        _maxHealth = _value;

        if (_healthSetter != null)
            _healthSetter.onEndEdit.AddListener(SetHealth);
    }

    private void OnDisable()
    {
        if (_healthSetter != null)
            _healthSetter.onEndEdit.AddListener(SetHealth);
    }

    public void SetInvincible(bool isInvincible)
    {
        _isInvinsible = isInvincible;
    }
    public void Decrease(float value, bool isCritical)
    {
        if(_isInvinsible)
            return;
        _value -= value;
 
         DamagePopup.Create(transform.position, (int)value, isCritical);
            
        if (_value <= 0)
        {
            Die();
            return;
        }
        Changed?.Invoke(_value);
    }

    public void Die()
    {
        _value = 0;

        Changed?.Invoke(_value);

        Died?.Invoke();

    }
    public void Increase(float value)
    {
        
        if ((_value + value) >= _maxHealth)
        {
            _value = _maxHealth;
            Changed?.Invoke(_value);
            return;
        }
        if(_healEffect != null)
            _healEffect.gameObject.SetActive(true);
        _value += value;
        Changed?.Invoke(_value);
    }
    public void SetHealth(string value)
    {
        if (value == null || value == " ")
            return;
        _value = float.Parse(value);
        if (_value <= 0)
        {
            _value = 0;
            Died?.Invoke();
        }
        Changed?.Invoke(_value);
    }
}