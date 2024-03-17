using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    private void OnDrawGizmos()
    {
        _cells = _gridLayout.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
    }
    private void Awake()
    {
        if (!manager)
        {
            manager = this;
        }
        else if (manager == this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _cells = _gridLayout.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
        //if (_cells.Count >= 12) CreateRandomItemsInInventory();
    }

    public void SetPlayer(Character player)
    {
        _player = player;
    }

    public void SetInventoryEquiped()
    {
        if (_player)
        {
            _player.gameObject.GetComponent<Equiper>().EquipWeapon(GetEquipedGun());
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
                SetInventoryEquiped();
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

    public WeaponData GetEquipedGun()
    {
        print(_gunInUse.GetComponentInChildren<ItemEquipable>().GetData());
        return _gunInUse.GetComponentInChildren<ItemEquipable>().GetData();
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
