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

    [Header("Для установки предмета со сцены")]
    [SerializeField] private bool _loadFromScene = false;
    [SerializeField] private itemRarity _rarity;
    [SerializeField] private itemType _itemType;
    [SerializeField] private int _itemLvl;
    [SerializeField] private float _mainParam;
    [SerializeField] private Sprite _itemImageSprite;

    [Header("Данные о типе огнестрела (if Gun)")]
    [SerializeField] private WeaponData _weapon;

    private Button _itemButton;
    [SerializeField]
    private bool _isEquiped;

    private void OnDrawGizmos()
    {
        if (_loadFromScene) SetItemBy(_itemType, _rarity, _itemLvl, _mainParam, _itemImageSprite, _weapon);
    }

    private void Awake()
    {
        _inventoryManager = InventoryItemManager.manager;
        _itemButton = GetComponent<Button>();
    }

    private void Start()
    {
        _itemButton.onClick.AddListener(UseItem);
    }


    public void SetItemBy(itemType type, itemRarity rarity, int Lvl, float mainParam, Sprite itemImage)
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
    public void SetItemBy(itemType type, itemRarity rarity, int Lvl, float mainParam, Sprite itemImage, WeaponData gunData) // Если автомат
    {
        // Функция для установки парамеров предмета из базы данных
        _rarity = rarity;
        _itemType = type;
        _itemLvl = Lvl;
        _mainParam = mainParam;
        _itemImageSprite = itemImage;

        _weapon = gunData;

        _itemTypeIcon.sprite = _inventoryManager.GetSpriteByType(_itemType);

        _itemRarityCircle.color = _inventoryManager.GetColorByRarity(_rarity);

        _textItemLvl.text = _itemLvl.ToString();

        _textMainParametr.text = _mainParam.ToString();

        _itemImage.sprite = _itemImageSprite;
    }
    public void SetItemBy(itemType type, itemRarity rarity, int Lvl, float mainParam) // Тестовая функция, удалить после реализации
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

    public void SetItemBy(ItemEquipable itemToCopy) // копирование из другого предмета
    {

        _itemType = itemToCopy.GetTypeItem();
        _itemTypeIcon.sprite = _inventoryManager.GetSpriteByType(_itemType);

        _weapon = itemToCopy.GetData();

        _rarity = itemToCopy.GetRarity();
        _itemRarityCircle.color = _inventoryManager.GetColorByRarity(_rarity);


        _itemLvl = itemToCopy.GetLvl();
        _textItemLvl.text = _itemLvl.ToString();

        _mainParam = itemToCopy.GetMainParam();
        _textMainParametr.text = _mainParam.ToString();

        _itemImageSprite = itemToCopy.GetSprite();
        _itemImage.sprite = _itemImageSprite;
    }

    public void ChangeToEquiped()
    {
        _isEquiped = true;
    }

    public void ChangeToUnequiped()
    {
        _isEquiped = false;
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

    public WeaponData GetData()
    {
        switch (_itemType)
        {
            case itemType.gun:
                return _weapon;
            default:
                return null;
        }
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
