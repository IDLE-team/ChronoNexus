using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DenaryHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _denaryText;

    private float _denaryValue;
    void Start()
    {
        _denaryValue = PlayerPrefs.GetFloat("denary", 0);
        _denaryText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.valuesChanged += OnValueChanged;
        PlayerProfileManager.profile.valuesChanged += SaveValue;
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        _denaryText.text = _denaryValue.ToString();
    }

    private void SaveValue()
    {
        PlayerPrefs.SetFloat("denary", _denaryValue);
    }

    public float GetValue()
    {
        return _denaryValue;
    }

    private void OnDestroy()
    {
        PlayerProfileManager.profile.valuesChanged -= OnValueChanged;
        PlayerProfileManager.profile.valuesChanged -= SaveValue;
        SaveValue();
    }

}
