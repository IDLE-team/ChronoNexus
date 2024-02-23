using System;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value;
    private float _maxHealth;
    public float MaxHealth => _maxHealth;
    [SerializeField] private TMP_InputField _healthSetter;

    /*[SerializeField] private float damageMultiplayerInShelter = 0.5f;
    public bool _inShelter;*/
    
    public float Value => _value;

    public event Action Died;

    public event Action<float> Changed;

    // healthSetter - дебаг штука, по-хорошему бы куда-то ещё закинуть её, но пока так
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

    public void Decrease(float value)
    {
        /*if (_inShelter)
        {
            _value = _value - (value * damageMultiplayerInShelter);
        }
        else
        {
            
        }*/
        _value -= value;

        if (_value <= 0)
        {
            _value = 0;
            Died?.Invoke();
        }
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