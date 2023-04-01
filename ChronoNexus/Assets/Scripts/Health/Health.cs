using System;
using UnityEngine;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value = 50f;
    
    public float Value => _value;
    public event Action Died;
    public event Action<float> Changed;
    public void Decrease(float value = 50)
    {
        _value -= value;
        if (_value <= 0)
        {
            _value = 0;
            Died?.Invoke();
        }
        Changed?.Invoke(_value);
    }
}