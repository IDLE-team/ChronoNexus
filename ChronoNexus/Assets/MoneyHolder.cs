using TMPro;
using UnityEngine;

public class MoneyHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Start()
    {
        _moneyText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.moneyChanged += OnMoneyChange;
        OnMoneyChange();
    }
    private void OnDrawGizmos()
    {
        _moneyText = GetComponent<TextMeshProUGUI>();
    }

    public void OnMoneyChange()
    {
        _moneyText.text = PlayerPrefs.GetFloat("money").ToString();
    }

    private void OnDestroy()
    {
        PlayerProfileManager.profile.moneyChanged -= OnMoneyChange;
    }
}
