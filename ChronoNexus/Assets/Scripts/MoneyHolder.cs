using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private float _moneyValue;

    private void Start()
    {
        _moneyValue = PlayerPrefs.GetFloat("money", 0);
        _moneyText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.valuesChanged += OnValueChanged;
        PlayerProfileManager.profile.valuesChanged += SaveValue;
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        _moneyText.text = _moneyValue.ToString();
    }

    private void SaveValue()
    {
        PlayerPrefs.SetFloat("money", _moneyValue);
    }

    public float GetMoneyValue()
    {
        return _moneyValue;
    }

    public void DecreaseMoneyValue(float cost)
    {
        _moneyValue -= cost;
        PlayerProfileManager.profile.valuesChanged();
    }


    private void OnDestroy()
    {
        PlayerProfileManager.profile.valuesChanged -= OnValueChanged;
        PlayerProfileManager.profile.valuesChanged -= SaveValue;
        SaveValue();
    }
}
