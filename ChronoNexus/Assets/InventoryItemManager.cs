using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemManager : MonoBehaviour
{
    public static InventoryItemManager manager;

    [Header("Иконки типов предмета")]
    [SerializeField]
    private List<Sprite> _itemTypeIcons = new List<Sprite>();

    [Header("Лэйаут предметов")]
    [SerializeField] private GameObject _gridLayout;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField]
    private List<HorizontalLayoutGroup> _cells = new List<HorizontalLayoutGroup>();

    [Header("Экипированные предметы")]
    [SerializeField] private GameObject _gunInUse;
    [SerializeField] private GameObject _knifeInUse;
    [SerializeField] private GameObject _granadeInUse;
    [SerializeField] private GameObject _armorInUse;

    private Character _player;
    private WeaponData _gunEquiped;
    [HideInInspector]
    public UnityAction OnCharacterLinked;

    private void OnDrawGizmos()
    {
        _cells = _gridLayout.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
    }
    private void OnEnable()
    {
        if (!manager)
        {
            manager = this;
        }
        else if (manager == this)
        {
            Destroy(this);
        }
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
        Debug.LogAssertion("да блять");
        if (_player)
        {
            if (_gunEquiped)
            {
                _player.gameObject.GetComponent<Equiper>().EquipWeapon(_gunEquiped);
            }
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
                //return new Color(135, 135, 135, 1);
                return new Color(0.5283019f, 0.5283019f, 0.5283019f, 1f);
            case itemRarity.green:
                //return new Color(3, 250, 162, 1);
                return new Color(0.01176471f, 0.9803922f, 0.6352941f, 1f);
            case itemRarity.purple:
                return new Color(0.4862745f, 0.4862745f, 0.9882354f, 1f);
            //return new Color(124, 124, 252, 1);
            case itemRarity.gold:
                return new Color(0.9882353f, 0.8862745f, 0.01176471f, 1f);
            //return new Color(252, 226, 3, 1);
            default:
                return new Color(0.5283019f, 0.5283019f, 0.5283019f, 1f);
        }
    }

    private void CreateRandomItemsInInventory() // тестовая функция
    {
        for (int i = 0; i < Random.Range(3, 10); i++)
        {
            var cell = Instantiate(_itemPrefab, _cells[i].transform);
            cell.GetComponent<ItemEquipable>().SetItemBy(
                (itemType)Random.Range(0, 4), (itemRarity)Random.Range(0, 4),
                Random.Range(1, 10), Random.Range(10, 50));
            print(cell.GetComponent<ItemEquipable>().GetMainParam());
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
        SetInventoryEquiped();
    }



    public void TradeParamentrs(GameObject itemInUse, ItemEquipable next)
    {
        ItemEquipable itemUse = itemInUse.GetComponentInChildren<ItemEquipable>();
        if (itemUse)
        {
            var item = Instantiate(itemUse);
            itemUse.SetItemBy(next);
            itemUse.ChangeToEquiped();
            next.SetItemBy(item);
            next.ChangeToUnequiped();
            Destroy(item.gameObject);

        }
        else //при установке первого предмета
        {
            var itemSet = Instantiate(_itemPrefab, itemInUse.transform);
            var itemSetted = itemSet.GetComponent<ItemEquipable>();
            itemSetted.SetItemBy(next);
            itemSetted.ChangeToEquiped();
            Destroy(next.gameObject);
        }
    }

    public void TradeParamentrs(ItemEquipable next)
    {
        GameObject itemEmpty = Instantiate(_itemPrefab); ;
        for (int i = 0; i < _cells.Count; i++)
        {
            if (!_cells[i].GetComponentInChildren<ItemEquipable>())
            {
                itemEmpty.transform.SetParent(_cells[i].transform);
                break;
            }
        }
        ItemEquipable itemUseEmpty = itemEmpty.GetComponent<ItemEquipable>();

        var item = Instantiate(itemUseEmpty);
        itemUseEmpty.SetItemBy(next);
        itemUseEmpty.ChangeToUnequiped();
        next.SetItemBy(item);
        next.ChangeToEquiped();
        Destroy(item.gameObject);
        Destroy(next.gameObject);

    }

    public WeaponData GetEquipedGun()
    {
        var equiped = _gunInUse.GetComponentInChildren<ItemEquipable>();
        print(equiped);
        if (equiped)
        {
            print(equiped);
            return equiped.GetData();
        }
        else
        {
            return null;
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
