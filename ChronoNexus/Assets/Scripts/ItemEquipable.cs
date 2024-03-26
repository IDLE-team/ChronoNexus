using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static InventoryItemManager;

public class ItemEquipable : MonoBehaviour
{

    [Header("Компоненты настройки предмета")]
    [SerializeField] private Image _itemImage;
    [SerializeField] private Image _itemRarityCircle;
    [SerializeField] private Image _itemTypeIcon;
    [SerializeField] private TextMeshProUGUI _textMainParametr;
    [SerializeField] private TextMeshProUGUI _textItemLvl;

    //[Header("Для установки предмета со сцены")]
    private itemRarity _rarity;
    private itemType _itemType;
    private int _itemLvl;
    private float _mainParam;
    private Sprite _itemImageSprite;

    [Header("Для установки предмета со сцены")]
    [SerializeField] private bool _loadFromScene = false;
    [SerializeField] private ItemData _itemData;

    private WeaponData _weapon; // если пушка

    private Button _itemButton;
    private bool _isEquiped;

    private InventoryItemManager manager;

    [SerializeField]
    private bool _isEquipedOnStart;

    [Inject]
    private void Construct(InventoryItemManager inventoryItemManager)
    {
        Debug.Log("ManagerInjected");
        manager = inventoryItemManager;
        print(manager.gameObject.name);
    }

    public void AddManager(InventoryItemManager _manager)
    {
        manager = _manager;
    }

    private void Start()
    {

        if (_isEquipedOnStart)
        {
            SetItem();
        }
        _itemButton = GetComponent<Button>();
        _itemButton.onClick.AddListener(() => SetItem());
    }

    private void Awake()
    {
    //    manager = GetComponentInParent<InventoryItemManager>();
        if (_loadFromScene)
        {
            SetItemBy(_itemData);
        }
    }

    public void SetGunItemBy(itemType type, itemRarity rarity, int Lvl, float mainParam, Sprite itemImage, ItemData Data)
    {
        // Функция для установки парамеров предмета из базы данных

        _itemData = Data;
        _rarity = rarity;
        _itemType = type;
        _itemLvl = Lvl;
        _mainParam = mainParam;
        _itemImageSprite = itemImage;

        _weapon = _itemData.weaponData;

        _itemTypeIcon.sprite = manager.GetSpriteByType(_itemType);

        _itemRarityCircle.color = manager.GetColorByRarity(_rarity);

        _textItemLvl.text = _itemLvl.ToString();

        _textMainParametr.text = _mainParam.ToString();

        _itemImage.sprite = _itemImageSprite;
    }

    public void SetItemBy(ItemEquipable itemToCopy) // копирование из другого предмета
    {
        SetItemBy(itemToCopy.GetItemData());
    }

    public void SetItemBy(ItemData itemToCopy) // копирование из другого предмета
    {
        print(itemToCopy.itemType);
        _itemData = itemToCopy;
        _itemType = itemToCopy.itemType;

        Debug.Log("Manager: " + manager);
        _itemTypeIcon.sprite = manager.GetSpriteByType(_itemType);

        _weapon = itemToCopy.weaponData; // тут надо будет дописывать - только под оружие сейча

        _rarity = itemToCopy.rarity;
        _itemRarityCircle.color = manager.GetColorByRarity(itemToCopy.rarity);


        _itemLvl = itemToCopy.itemLvl;
        _textItemLvl.text = _itemLvl.ToString();

        _mainParam = itemToCopy.weaponData.Damage; // тоже только под оружие
        _textMainParametr.text = _mainParam.ToString();

        _itemImageSprite = itemToCopy.itemImageSprite;
        _itemImage.sprite = _itemImageSprite;

    }

    public void SetItemBy(ItemData itemToCopy, InventoryItemManager inventoryItemManager) // копирование из другого предмета
    {
        manager = inventoryItemManager;
        SetItemBy(itemToCopy);
    }

    public void ChangeToEquiped()
    {
        _isEquiped = true;
        Debug.Log(manager);
        manager.SetInventoryEquiped();
    }

    public void ChangeToUnequiped()
    {
        _isEquiped = false;
    }

    public void SetItem()
    {
        if (!_isEquiped) // to equip inventory
        {
            manager.EquipItem(GetTypeItem(), this);
            manager.SetInventoryEquiped();
        }
        else // to set back to inventory
        {

            manager.TradeParametersToEmptyFromEquiped(this);
            manager.SetInventoryEquiped();
        }
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

    public WeaponData GetDataByType()
    {
        switch (_itemType)
        {
            case itemType.gun:
                return _weapon;
            default:
                return null;
        }
    }

    public ItemData GetItemData()
    {
        return _itemData;
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
