using TMPro;
using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private int _moneyValue;

    private void Start()
    {
        _moneyValue = PlayerPrefs.GetInt("money", 0);
        _moneyText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.moneyChanged += OnMoneyChange;
        PlayerProfileManager.profile.moneyChanged += SaveMoney;
        OnMoneyChange();
    }

    public void OnMoneyChange()
    {
        _moneyText.text = _moneyValue.ToString();
        SaveMoney();
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("money", _moneyValue);
    }

    public int GetMoneyValue()
    {
        return _moneyValue;
    }
    public bool DecreaseMoneyValue(int ValueToDecrease)
    {
        if (_moneyValue - ValueToDecrease >= 0 && ValueToDecrease > 0)
        {
            PlayerProfileManager.profile.moneyChanged();
            _moneyValue -= ValueToDecrease;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IncreaseMoney(int ValueToIncrease)
    {
        if (ValueToIncrease >= 0)
        {
            _moneyValue += ValueToIncrease;
            PlayerProfileManager.profile.moneyChanged();
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnDestroy()
    {
        PlayerProfileManager.profile.moneyChanged -= OnMoneyChange;
        PlayerProfileManager.profile.moneyChanged -= SaveMoney;
        SaveMoney();
    }
}
