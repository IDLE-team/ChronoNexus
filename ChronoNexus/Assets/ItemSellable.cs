using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellable : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    [SerializeField] private TextMeshProUGUI _mainParamText;
    [SerializeField] private TextMeshProUGUI _LevelText;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _itemRarityText;

    [SerializeField] private TextMeshProUGUI _itemParam1;
    [SerializeField] private TextMeshProUGUI _itemParam2;
    [SerializeField] private TextMeshProUGUI _itemParam3;
    [SerializeField] private Image _itemIconType;
    [SerializeField] private Image _itemRarityCircle;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Button _purchaseButton;

    private void Awake()
    {
        _purchaseButton.onClick.AddListener(Purchase);
        SetSellableItem();
    }

    private void Purchase()
    {
        InventoryItemManager.manager.BuyItem(_itemData.itemCost);
        InventoryItemManager.manager.MakeItemFromShop(_itemData);
        gameObject.SetActive(false);
    }

    private void SetSellableItem()
    {
        _mainParamText.text = _itemData.weaponData.Damage.ToString();
        _LevelText.text = _itemData.itemLvl.ToString();
        _itemNameText.text = _itemData.itemName;
        _costText.text = _itemData.itemCost.ToString();
        _itemImage.sprite = _itemData.itemImageSprite;
        _itemIconType.sprite = InventoryItemManager.manager.GetSpriteByType(_itemData.itemType);
        _itemRarityCircle.color = InventoryItemManager.manager.GetColorByRarity(_itemData.rarity);
        _itemRarityText.text = InventoryItemManager.manager.GetTextByRarity(_itemData.rarity);
        switch (_itemData.itemType)
        {
            case InventoryItemManager.itemType.gun:
                var gun = _itemData.weaponData;
                _itemParam1.text = "Урон  " + gun.Damage.ToString();
                _itemParam2.text = "Ск.атаки  " + gun.FireRate.ToString();
                _itemParam3.text = "Обойма  " + gun.MaxAmmo.ToString();
                break;
            case InventoryItemManager.itemType.armor:
                break;
            case InventoryItemManager.itemType.knife:
                break;
            case InventoryItemManager.itemType.granade:
                break;

        }
    }

}
