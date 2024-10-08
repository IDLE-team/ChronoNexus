using TMPro;
using UnityEngine;

public class LootCard : MonoBehaviour
{
    [Header("Деньги")]
    [SerializeField] private GameObject _typeMoney;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [Header("Материалы")]
    [SerializeField] private GameObject _typeMaterial;
    [SerializeField] private TextMeshProUGUI _materialText;
    [Header("Опыт")]
    [SerializeField] private GameObject _typeExp;
    [SerializeField] private TextMeshProUGUI _expText;
    [Header("Оружие")]
    [SerializeField] private GameObject _typeGun;

    private void Start()
    {

        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 750);
    }

    public void DisplayLootData(HubIventoryManager.itemRarity rarity)
    {
        _typeGun.SetActive(true);
    }

    public void DisplayLootData(HubIventoryManager.lootType lootType, int amount)
    {
        switch (lootType)
        {
            case HubIventoryManager.lootType.money:

                _typeMoney.SetActive(true);
                HubIventoryManager.manager.GetMoneyHolder().IncreaseMoney(amount);
                _moneyText.text = amount.ToString();

                break;

            case HubIventoryManager.lootType.material:

                _typeMaterial.SetActive(true);
                HubIventoryManager.manager.GetMaterialHolder().IncreaseMaterialValue(amount);
                _materialText.text = amount.ToString();

                break;

            case HubIventoryManager.lootType.exp:

                _typeExp.SetActive(true);

                _expText.text = amount.ToString();

                break;
        }
    }
}
