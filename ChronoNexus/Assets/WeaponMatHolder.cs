using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponMatHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weaponMatText;

    private float _weaponMat;
    void Start()
    {
        _weaponMat = PlayerPrefs.GetFloat("weaponMaterials", 0);
        _weaponMatText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.valuesChanged += OnValueChanged;
        PlayerProfileManager.profile.valuesChanged += SaveValue;
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        _weaponMatText.text = _weaponMat.ToString();
    }

    private void SaveValue()
    {
        PlayerPrefs.SetFloat("weaponMaterials", _weaponMat);
    }

    public float GetValue()
    {
        return _weaponMat;
    }

    private void OnDestroy()
    {
        PlayerProfileManager.profile.valuesChanged -= OnValueChanged;
        PlayerProfileManager.profile.valuesChanged -= SaveValue;
        SaveValue();
    }
}
