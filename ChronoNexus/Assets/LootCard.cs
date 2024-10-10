using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private TextMeshProUGUI _mainParamText;
    [SerializeField] private TextMeshProUGUI _LevelText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _itemIconType;
    [SerializeField] private Image _itemRarityCircle;

    private void Start()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(450, 750);
    }

    public void DisplayLootData(ItemData itemData) // weaponOnly
    {
        _typeGun.SetActive(true);
        HubIventoryManager.manager.MakeItemFromShop(itemData);

        _mainParamText.text = itemData.weaponData.Damage.ToString();
        _LevelText.text = itemData.itemLvl.ToString();

        _itemImage.sprite = itemData.itemImageSprite;
        _itemIconType.sprite = HubIventoryManager.manager.GetSpriteByType(itemData.itemType);
        _itemRarityCircle.color = HubIventoryManager.manager.GetColorByRarity(itemData.rarity);

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

        }
    }

    public void DisplayLootData(float minShare, float maxShare) // expOnly
    {
        _typeExp.SetActive(true);
        int exp = (int)HubIventoryManager.manager.GetLevelHolder().AddExp(minShare, maxShare);
        _expText.text = exp.ToString();
    }
}
