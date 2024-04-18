using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value;
    private float _maxHealth;
    public float MaxHealth => _maxHealth;
    [SerializeField] private TMP_InputField _healthSetter;
    public float Value => _value;

    public event Action Died;

    public event Action<float> Changed;
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

    public void Decrease(float value, bool isCritical)
    {
        _value -= value;
 
         DamagePopup.Create(transform.position, (int)value, isCritical);
            
        if (_value <= 0)
        {
            _value = 0;
            Died?.Invoke();
        }
        Changed?.Invoke(_value);
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