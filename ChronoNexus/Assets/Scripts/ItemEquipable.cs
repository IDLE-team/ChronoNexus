using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private bool _isEquiped;

    private InventoryItemManager manager;

    [SerializeField]
    private bool _isEquipedOnStart;


    [Inject]
    private void Construct(InventoryItemManager inventoryItemManager)
    {
        manager = inventoryItemManager;
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
        if (_itemButton.interactable)
        {
            _itemButton.onClick.AddListener(() => SetItem());
            _itemButton.onClick.AddListener(() => Pressed());
        }
    }

    private void Awake()
    {
        //    manager = GetComponentInParent<InventoryItemManager>();
        if (_loadFromScene)
        {
            SetItemBy(_itemData);
        }
    }

    private void Pressed()
    {
        PlayerProfileManager.profile.itemChanged();
    }

    public void SetItemBy(ItemEquipable itemToCopy) // копирование из другого предмета
    {
        SetItemBy(itemToCopy.GetItemData());
    }

    public void SetItemBy(ItemData itemData)
    {
        _itemData = itemData;
        _itemType = _itemData.itemType;

        _itemTypeIcon.sprite = manager.GetSpriteByType(_itemType);

        _weapon = _itemData.weaponData; // тут надо будет дописывать - только под оружие сейча

        _rarity = _itemData.rarity;
        _itemRarityCircle.color = manager.GetColorByRarity(_itemData.rarity);


        _itemLvl = _itemData.itemLvl;
        _textItemLvl.text = _itemLvl.ToString();

        _mainParam = _itemData.weaponData.Damage; // тоже только под оружие
        _textMainParametr.text = _mainParam.ToString();

        _itemImageSprite = _itemData.itemImageSprite;
        _itemImage.sprite = _itemImageSprite;

        print(_itemImage.sprite.name + _itemData.weaponData.WeaponName);

    }

    public void SetItemBy(ItemData itemToCopy, InventoryItemManager inventoryItemManager) // копирование из другого предмета
    {
        manager = inventoryItemManager;
        SetItemBy(itemToCopy);
    }

    public void ChangeToEquiped()
    {
        _isEquiped = true;
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
            if (HubIventoryManager.manager) // если в хабе
            {
                // manager.EquipItem(GetTypeItem(), this);
                manager.TradeParamentrs(HubIventoryManager.manager.GetGunCell().gameObject,this);
                manager.SetInventoryEquiped();
            }
            else // на уровне
            {
                manager.EquipItem(GetTypeItem(), this);
                manager.SetInventoryEquiped();
            }
        }
        else // to set back to inventory
        {
            if (HubIventoryManager.manager) // если в хабе
            {
                if (PlayerPrefs.GetString("inventoryMain", "").Split(' ').Length < 8)
                {
                    manager.TradeParametersToEmptyFromEquiped(this);
                }
                else
                {
                    var item = HubIventoryManager.manager.GetInventoryCell(0).gameObject.GetComponentInChildren<ItemEquipable>();
                    manager.TradeParamentrs(this, item);
                    manager.SetInventoryEquiped();
                }
            }
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

    public void SetItemData(ItemData itemData)
    {
        _itemData = itemData;
    }

    public void GetAllParamentrs(out itemType type, out itemRarity rarity, out int lvl, out float mainParam, out Sprite sprite)
    {
        type = _itemType;
        rarity = _rarity;
        lvl = _itemLvl;
        mainParam = _mainParam;
        sprite = _itemImageSprite;
    }

    public void SetEqipedOnLoad()
    {
        _isEquiped = true;
    }
}
