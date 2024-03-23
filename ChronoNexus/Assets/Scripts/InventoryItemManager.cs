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
    private List<Sprite> _itemTypeIcons = new List<Sprite>();

    [Header("Лэйаут предметов")]
    [SerializeField] private GameObject _gridLayout;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField]
    private List<HorizontalLayoutGroup> _cells = new List<HorizontalLayoutGroup>();

    [Header("Точки Экипировки")]
    [SerializeField] private GameObject _gunInUse;
    [SerializeField] private GameObject _knifeInUse;
    [SerializeField] private GameObject _granadeInUse;
    [SerializeField] private GameObject _armorInUse;

    private Character _player;
    private WeaponData _gunEquiped;

    [HideInInspector]
    public UnityAction OnCharacterLinked;

    [SerializeField] private MoneyHolder _moneyHolder;

    public event Action<WeaponData> OnEquiped;
    private ItemEquipable itemUse;
    private void OnDrawGizmos()
    {
        _cells = _gridLayout.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
    }
    private void OnEnable()
    {
        /*if (!manager)
        {
            manager = this;
        }
        else if (manager == this)
        {
            Destroy(this);
        }*/
        OnCharacterLinked += SetInventoryEquiped;
    }

    private void Start()
    {
        _cells = _gridLayout.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
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
        //if (_player)
       // {
            if (_gunEquiped)
            {
              //  _player.gameObject.GetComponent<Equiper>().EquipWeapon(_gunEquiped);
              Debug.Log("EqipeCalled");
              OnEquiped?.Invoke(_gunEquiped);
            }
       // }
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
        //SetInventoryEquiped();
    }


    public void TradeParamentrs(GameObject itemInUse, ItemEquipable next)
    {
      //  itemUse = itemInUse.GetComponentInChildren<ItemEquipable>();
       if (itemUse)
        {
            itemUse.ChangeToUnequiped();
            MoveToGeneralInventory();
            next.transform.parent = itemInUse.transform;
            next.ChangeToEquiped();
            itemUse = next;
            // var item = Instantiate(itemUse);
            //itemUse.SetItemBy(next);
            //itemUse.ChangeToEquiped();
            //next.SetItemBy(item);
            //print(item.name);
            //Destroy(item.gameObject);
        }
        else //при установке первого предмета
        {
            next.transform.parent = itemInUse.transform;
            next.ChangeToEquiped();
            itemUse = next;
            //var itemSet = Instantiate(_itemPrefab, itemInUse.transform);
            //var itemSetted = itemSet.GetComponent<ItemEquipable>();
            //itemSetted.SetItemBy(next);
            //Destroy(next.gameObject);
        }
    }

    public void MoveToGeneralInventory()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (!_cells[i].GetComponentInChildren<ItemEquipable>())
            {
                itemUse.transform.SetParent(_cells[i].transform);
                break;
            }
        }
    }
    public void MoveToGeneralInventory(GameObject itemEmpty)
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (!_cells[i].GetComponentInChildren<ItemEquipable>())
            {
                itemEmpty.transform.SetParent(_cells[i].transform);
                break;
            }
        }
    }
    public void TradeParametersToEmptyFromEquiped(ItemEquipable next)
    {
       // var item = SpawnEmptyItem();
       // item.SetItemBy(next);
       itemUse.ChangeToUnequiped();
       // next.SetItemBy(item);
       MoveToGeneralInventory();

       if (next != itemUse)
       {
           next.transform.parent = _gunInUse.transform;
           next.ChangeToEquiped();
       }
       else
       {
           // itemUse.transform.parent
           itemUse = null;
       }
       
       // Destroy(next.gameObject);
    }

    private ItemEquipable SpawnEmptyItem()
    {
        GameObject itemEmpty = Instantiate(_itemPrefab);
        MoveToGeneralInventory();

        ItemEquipable itemUseEmpty = itemEmpty.GetComponent<ItemEquipable>();
        return itemUseEmpty;
    }
    private ItemEquipable SpawnEmptyItem(ItemData ItemData)//говно ебаное всё нахуй снести тут, насрал я
    {
        GameObject itemEmpty = Instantiate(_itemPrefab);
        MoveToGeneralInventory(itemEmpty);

        ItemEquipable itemUseEmpty = itemEmpty.GetComponent<ItemEquipable>();
        return itemUseEmpty;
    }

    public void MakeItemFromShop(ItemData soldItem)
    {
        SpawnEmptyItem().SetItemBy(soldItem);
    }
    public void AddItem(ItemData ItemData)
    {
        ItemEquipable item = SpawnEmptyItem(ItemData);
        item.SetWithManagerItemBy(ItemData,this);
        item.GetComponent<Button>().onClick.AddListener(() => item.SetItem());
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
