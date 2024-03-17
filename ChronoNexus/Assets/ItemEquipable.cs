using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static InventoryItemManager;

public class ItemEquipable : MonoBehaviour
{
    private InventoryItemManager _inventoryManager;

    [Header("Компоненты настройки предмета")]
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _itemRarityCircle;
    [SerializeField] private Image _itemTypeIcon;
    [SerializeField] private TextMeshProUGUI _textMainParametr;
    [SerializeField] private TextMeshProUGUI _textItemLvl;

    private itemRarity _rarity;
    private itemType _itemType;
    private int _itemLvl;
    private float _mainParam;
    [SerializeField] private Sprite _itemImageSprite;

    private Button _itemButton;

    private void OnDrawGizmos()
    {
        _inventoryManager = GetComponentInParent<InventoryItemManager>();
    }

    private void Awake()
    {
        _inventoryManager = GetComponentInParent<InventoryItemManager>();
        _itemButton = GetComponent<Button>();
    }

    private void Start()
    {

       // if (_inventoryManager) SetItem(_itemType, _rarity, _itemLvl, _mainParam, _itemImageSprite);
        _itemButton.onClick.AddListener(UseItem);
    }


    public void SetItem(itemType type, itemRarity rarity, int Lvl, float mainParam, Sprite itemImage)
    {
        // Функция для установки парамеров предмета из базы данных
        _rarity = rarity;
        _itemType = type;
        _itemLvl = Lvl;
        _mainParam = mainParam;
        _itemImageSprite = itemImage;

        _itemTypeIcon.sprite = _inventoryManager.GetSpriteByType(_itemType);

        _itemRarityCircle.color = _inventoryManager.GetColorByRarity(_rarity);

        _textItemLvl.text = _itemLvl.ToString();

        _textMainParametr.text = _mainParam.ToString();

        _itemImage.sprite = _itemImageSprite;
    }
    public void SetItem(itemType type, itemRarity rarity, int Lvl, float mainParam) // Тестовая функция, удалить после реализации
    {
        // Функция для установки парамеров предмета из базы данных
        _rarity = rarity;
        _itemType = type;
        _itemLvl = Lvl;
        _mainParam = mainParam;

        _itemTypeIcon.sprite = _inventoryManager.GetSpriteByType(_itemType);

        _itemRarityCircle.color = _inventoryManager.GetColorByRarity(_rarity);

        _textItemLvl.text = _itemLvl.ToString();

        _textMainParametr.text = _mainParam.ToString();

        _itemImage.sprite = _itemImageSprite;
    }

    public void SetItem(ItemEquipable itemToCopy) // копирование из другого предмета
    {

        _itemType = itemToCopy.GetTypeItem();
        _itemTypeIcon.sprite = _inventoryManager.GetSpriteByType(_itemType);

        _rarity = itemToCopy.GetRarity();
        _itemRarityCircle.color = _inventoryManager.GetColorByRarity(_rarity);


        _itemLvl = itemToCopy.GetLvl();
        _textItemLvl.text = _itemLvl.ToString();

        _mainParam = itemToCopy.GetMainParam();
        _textMainParametr.text = _mainParam.ToString();

        _itemImage.sprite = itemToCopy.GetSprite();
    }

    private void UseItem()
    {
        _inventoryManager.EquipItem(GetTypeItem(), this);
    }

    public itemType GetTypeItem()
    {
        return _itemType;
    }
    public itemRarity GetRarity()
    {
        return _rarity;
    }
    public int GetLvl()
    {
        return _itemLvl;
    }
    public float GetMainParam()
    {
        return _mainParam;
    }
    public Sprite GetSprite()
    {
        return _itemImageSprite;
    }

    public void GetAllParamentrs(out itemType type, out itemRarity rarity, out int lvl, out float mainParam, out Sprite sprite)
    {
        type = _itemType;
        rarity = _rarity;
        lvl = _itemLvl;
        mainParam = _mainParam;
        sprite = _itemImageSprite;
    }

}
