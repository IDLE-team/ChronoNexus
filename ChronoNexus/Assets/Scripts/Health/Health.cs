using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] private float _value;
    [SerializeField] private TMP_InputField _healthSetter;
    [SerializeField] private ParticleSystem _healEffect;
    [SerializeField] private bool _isPlayer;
    public IEnumerator DebugMax()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log(MaxHealth);
    }

[SerializeField]
    private float _maxHealth;

    public float MaxHealth => GetMaxHealth();
    public float Value => _value;

    public event Action Died;

    public event Action<float> Changed;

    private bool _isInvinsible;

    public bool IsInvincible => _isInvinsible;

    public float GetMaxHealth()
    {
        if(_isPlayer && UpgradeData.Instance)
            return _maxHealth + UpgradeData.Instance.MaxHPUpgradeValue;
        return _maxHealth;
    }
    private void OnEnable()
    {
        _value = GetMaxHealth();
        if (_healthSetter != null)
            _healthSetter.onEndEdit.AddListener(SetHealth);
        StartCoroutine(DebugMax());
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
        Debug.Log("HealthDie");
        _value = 0;

        Changed?.Invoke(_value);

        Died?.Invoke();

    }
    public void Increase(float value)
    {
        
        if ((_value + value) >= GetMaxHealth())
        {
            _value = GetMaxHealth();
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