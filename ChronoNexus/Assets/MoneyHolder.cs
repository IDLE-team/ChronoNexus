using TMPro;
using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private float _moneyValue;

    private void Start()
    {
        _moneyValue = PlayerPrefs.GetFloat("money",0);
        _moneyText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.moneyChanged += OnMoneyChange;
        PlayerProfileManager.profile.moneyChanged += SaveMoney;
        OnMoneyChange();
    }

    public void OnMoneyChange()
    {
        _moneyText.text = _moneyValue.ToString();
    }

    private void SaveMoney()
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
        PlayerProfileManager.profile.moneyChanged();
    }


    private void OnDestroy()
    {
        PlayerProfileManager.profile.moneyChanged -= OnMoneyChange;
        PlayerProfileManager.profile.moneyChanged -= SaveMoney;
        SaveMoney();
    }
}
