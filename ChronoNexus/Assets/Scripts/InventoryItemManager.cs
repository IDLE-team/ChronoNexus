using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemManager : MonoBehaviour
{
    //public static InventoryItemManager manager;

    [Header("Иконки типов предмета")]
    [SerializeField]
    protected List<Sprite> _itemTypeIcons = new List<Sprite>();

    [Header("Лэйауты предметов")]
    [SerializeField] protected GameObject _gridLayoutTabInventory;


    [SerializeField] protected GameObject _itemPrefab;
    protected List<HorizontalLayoutGroup> _cellsInventory = new List<HorizontalLayoutGroup>();

    [Header("Точки Экипировки")]
    [SerializeField] protected GameObject _gunInUse;
    [SerializeField] protected GameObject _knifeInUse;
    [SerializeField] protected GameObject _granadeInUse;
    [SerializeField] protected GameObject _armorInUse;

    protected Character _player;
    protected WeaponData _gunEquiped;

    [HideInInspector]
    protected UnityAction OnCharacterLinked;
    protected UnityAction OnInventoryChanged;

    [SerializeField] protected MoneyHolder _moneyHolder;

    public event Action<WeaponData> OnEquiped;

    protected ItemEquipable itemUse;
    private void OnEnable()
    {
        OnCharacterLinked += SetInventoryEquiped;

    }
    private void Awake()
    {
        DeleteInventory();
    }
    private void Start()
    {
        OnInventoryChanged += SaveInventory;
        _cellsInventory = _gridLayoutTabInventory.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
        LoadInventory();
    }

    public void SetPlayer(Character player)
    {
        _player = player;
        OnCharacterLinked();
    }

    public Character GetPlayer()
    {
        if (_player)
        {
            return _player;
        }
        else
        {
            return null;
        }
    }

    public void SetInventoryEquiped()
    {
        _gunEquiped = GetEquipedGun();

        if (_gunEquiped)
        {
            //  _player.gameObject.GetComponent<Equiper>().EquipWeapon(_gunEquiped);
            OnEquiped?.Invoke(_gunEquiped);
        }
    }
    public Sprite GetSpriteByType(itemType itemType)
    {
        switch (itemType)
        {
            case itemType.gun:
                return _itemTypeIcons[1];
            case itemType.knife:
                return _itemTypeIcons[2];
            case itemType.armor:
                return _itemTypeIcons[3];
            case itemType.granade:
                return _itemTypeIcons[4];
            default:
                return _itemTypeIcons[0];
        }
    }

    public Color GetColorByRarity(itemRarity itemRarity)
    {
        switch (itemRarity)
        {
            case itemRarity.gray:
                return new Color(0.5283019f, 0.5283019f, 0.5283019f, 1f);
            case itemRarity.green:
                return new Color(0.01176471f, 0.9803922f, 0.6352941f, 1f);
            case itemRarity.purple:
                return new Color(0.4862745f, 0.4862745f, 0.9882354f, 1f);
            case itemRarity.gold:
                return new Color(0.9882353f, 0.8862745f, 0.01176471f, 1f);
            default:
                return new Color(0.5283019f, 0.5283019f, 0.5283019f, 1f);
        }
    }

    public string GetTextByRarity(itemRarity itemRarity)
    {
        switch (itemRarity)
        {
            case itemRarity.gray:
                return "Обычный";
            case itemRarity.green:
                return "Необычный";
            case itemRarity.purple:
                return "Редкий";
            case itemRarity.gold:
                return "Легендарный";
            default:
                return "Обычный";
        }
    }
    public void EquipItem(itemType itemType, ItemEquipable item)
    {
        switch (itemType)
        {
            case itemType.gun:
                TradeParamentrs(_gunInUse, item);
                return;
            case itemType.knife:
                TradeParamentrs(_knifeInUse, item);
                return;
            case itemType.granade:
                TradeParamentrs(_granadeInUse, item);
                return;
            case itemType.armor:
                TradeParamentrs(_armorInUse, item);
                return;
        }
    }


    public void TradeParamentrs(GameObject itemInUse, ItemEquipable next)
    {
        if (itemUse)
        {
            itemUse.ChangeToUnequiped();
            MoveToGeneralInventory();
            next.transform.parent = itemInUse.transform;
            next.ChangeToEquiped();
            itemUse = next;
        }
        else //при установке первого предмета
        {
            next.transform.parent = itemInUse.transform;
            next.ChangeToEquiped();
            itemUse = next;
        }
        OnInventoryChanged();
    }

    public void MoveToGeneralInventory()
    {
        for (int i = 0; i < _cellsInventory.Count; i++)
        {

            if (!_cellsInventory[i].GetComponentInChildren<ItemEquipable>())
            {
                itemUse.transform.SetParent(_cellsInventory[i].transform);
                break;
            }
        }
        OnInventoryChanged();
    }
    public void MoveToGeneralInventory(GameObject itemEmpty)
    {
        for (int i = 0; i < _cellsInventory.Count; i++)
        {
            print(i);
            if (!_cellsInventory[i].GetComponentInChildren<ItemEquipable>())
            {
                itemEmpty.transform.SetParent(_cellsInventory[i].transform);
                break;
            }
        }
        OnCharacterLinked();
    }
    public void TradeParametersToEmptyFromEquiped(ItemEquipable next)
    {

        itemUse.ChangeToUnequiped();
        MoveToGeneralInventory();

        if (next != itemUse)
        {
            next.transform.parent = _gunInUse.transform;
            next.ChangeToEquiped();
        }
        else
        {
            itemUse = null;
        }
        OnInventoryChanged();
    }

    public void SaveInventory()
    {
        var saveString = "";
        for (int i = 0; i < _cellsInventory.Count; i++)
        {
            var item = _cellsInventory[i].GetComponentInChildren<ItemEquipable>();
            if (item == null)
            {
                saveString.TrimEnd();
                break;
            }
            saveString += ItemDataManager.itemManager.GetIndexByItemData(item.GetItemData()).ToString() + " ";
        }

        PlayerPrefs.SetString("inventoryMain", saveString);
    }

    public void LoadInventory() // только на старте
    {

        SetLoadInventory();

    }

    private void SetLoadInventory()
    {
        var savedData = PlayerPrefs.GetString("inventoryMain", "");
        var listOfItems = savedData.Split(' ');
        List<int> items = new List<int>();
        for (int i = 0; i < listOfItems.Length - 1; i++)
        {
            print(listOfItems[i]);
            items.Add(Convert.ToInt32(listOfItems[i]));
        }
        for (int i = 0; i < items.Count; i++)
        {
            var data = ItemDataManager.itemManager.GetItemDataByIndex(items[i]);
            AddItem(data);
        }
    }

    public void DeleteInventory() // не трогать если жизнь дорога
    {
        var invent = _gridLayoutTabInventory.GetComponentsInChildren<ItemEquipable>();
        foreach (var item in invent)
        {
            Destroy(item.gameObject);
        }
    }

    private ItemEquipable SpawnEmptyItem()
    {
        GameObject itemEmpty = Instantiate(_itemPrefab);
        MoveToGeneralInventory(itemEmpty);

        ItemEquipable itemUseEmpty = itemEmpty.GetComponent<ItemEquipable>();
        return itemUseEmpty;
    }

    public void MakeItemFromShop(ItemData soldItem)
    {
        SpawnEmptyItem().SetItemBy(soldItem, this);
        OnInventoryChanged();
    }
    public void AddItem(ItemData ItemData)
    {
        ItemEquipable item = SpawnEmptyItem();
        item.SetItemBy(ItemData, this);
        OnInventoryChanged();
    }

    public WeaponData GetEquipedGun()
    {
        var equiped = _gunInUse.GetComponentInChildren<ItemEquipable>();
        print(equiped);
        if (equiped)
        {
            print(equiped);
            return equiped.GetDataByType();
        }
        else
        {
            return null;
        }
    }

    public bool BuyItem(float itemCost)
    {
        return true;
        if (_moneyHolder.GetMoneyValue() - itemCost >= 0)
        {
            _moneyHolder.DecreaseMoneyValue(itemCost);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDisable()
    {
        OnCharacterLinked -= SetInventoryEquiped;
        OnInventoryChanged -= SaveInventory;
    }

    public enum itemType
    {
        gun, knife, armor, granade
    }

    public enum itemRarity
    {
        gray, green, purple, gold
    }
}
