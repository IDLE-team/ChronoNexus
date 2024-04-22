using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value;
    [SerializeField] private TMP_InputField _healthSetter;
    
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