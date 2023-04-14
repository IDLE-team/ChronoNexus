using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value;
    [SerializeField] private TMP_InputField _healthSetter;
    public float Value => _value;

    public event Action Died;

    public event Action<float> Changed;

    private void OnEnable()
    {
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