using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
    [SerializeField] private InventoryItemManager manager;

        
    [Inject]
    private void Construct(InventoryItemManager inventoryItemManager)
    {
        Debug.Log("ManagerInjected");
        manager = inventoryItemManager;
    }
    private void Awake()
    {
        _purchaseButton.onClick.AddListener(Purchase);
        SetSellableItem();
    }

    private void Purchase()
    {
       manager.BuyItem(_itemData.itemCost);
       manager.MakeItemFromShop(_itemData);
        gameObject.SetActive(false);
    }

    private void SetSellableItem()
    {
        _mainParamText.text = _itemData.weaponData.Damage.ToString();
        _LevelText.text = _itemData.itemLvl.ToString();
        _itemNameText.text = _itemData.itemName;
        _costText.text = _itemData.itemCost.ToString();
        _itemImage.sprite = _itemData.itemImageSprite;
        _itemIconType.sprite =manager.GetSpriteByType(_itemData.itemType);
        _itemRarityCircle.color = manager.GetColorByRarity(_itemData.rarity);
        _itemRarityText.text = manager.GetTextByRarity(_itemData.rarity);
        switch (_itemData.itemType)
        {
            case InventoryItemManager.itemType.gun:
                var gun = _itemData.weaponData;
                _itemParam1.text = "����  " + gun.Damage.ToString();
                _itemParam2.text = "��.�����  " + gun.FireRate.ToString();
                _itemParam3.text = "������  " + gun.MaxAmmo.ToString();
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
