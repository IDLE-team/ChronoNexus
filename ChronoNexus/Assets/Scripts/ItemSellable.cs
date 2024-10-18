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

    [SerializeField] private GameObject _taken;

    private HubIventoryManager _manager;


    private void Awake()
    {
        _purchaseButton.onClick.AddListener(Purchase);

        _manager = HubIventoryManager.manager;
        SetSellableItem();
    }

    private void Start()
    {
        PlayerProfileManager.profile.moneyChanged += PurchaseButtonActive;
        PlayerProfileManager.profile.itemChanged += PurchaseButtonActive;
        PurchaseButtonActive();
    }

    private void PurchaseButtonActive()
    {
        var item = ItemDataManager.itemManager.GetIndexByItemData(_itemData);
        if (_manager.ContainsIndex(PlayerPrefs.GetString("inventoryMain", ""), item) && PlayerPrefs.GetInt("gun") == item)
        {
            _taken.SetActive(true);
            _purchaseButton.gameObject.SetActive(false);
            return;
        }

        if (HubIventoryManager.manager.GetMoneyValue() >= _itemData.itemCost)
        {
            _purchaseButton.interactable = true;
        }
        else
        {
            _purchaseButton.interactable = false;
        }
    }

    private void Purchase()
    {
        if (HubIventoryManager.manager.BuyItem(_itemData.itemCost))
        {
            PlayerProfileManager.profile.OnMoneyChange();

            GiftDropTab.instance.SetGift(_itemData);
            gameObject.SetActive(false);
        }
    }

    private void SetSellableItem()
    {
        _mainParamText.text = _itemData.weaponData.Damage.ToString();
        _LevelText.text = _itemData.itemLvl.ToString();
        _itemNameText.text = _itemData.itemName;
        _costText.text = _itemData.itemCost.ToString();

        _itemImage.sprite = _itemData.itemImageSprite;
        _itemIconType.sprite = _manager.GetSpriteByType(_itemData.itemType);
        _itemRarityCircle.color = _manager.GetColorByRarity(_itemData.rarity);

        _itemRarityText.text = _manager.GetTextByRarity(_itemData.rarity);

        switch (_itemData.itemType)
        {
            case InventoryItemManager.itemType.gun:
                var gun = _itemData.weaponData;
                _itemParam1.text = "Сила атаки  " + gun.Damage.ToString();
                _itemParam2.text = "Скорость Атаки  " + gun.FireRate.ToString();
                _itemParam3.text = "Обойма  " + gun.MaxAmmo.ToString();
                break;
                //  case InventoryItemManager.itemType.armor:
                //      break;
                //  case InventoryItemManager.itemType.knife:
                //      break;
                //  case InventoryItemManager.itemType.granade:
                //      break;

        }
    }

    private void OnDestroy()
    {

        PlayerProfileManager.profile.moneyChanged -= PurchaseButtonActive;
        PlayerProfileManager.profile.itemChanged -= PurchaseButtonActive;
    }
}
