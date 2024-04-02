using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HubIventoryManager : InventoryItemManager
{
    public static HubIventoryManager manager;

    [SerializeField] private GameObject _gridLayoutShelterInventory;
    [SerializeField] private GameObject _gridLayoutShelterStorage;

    private List<HorizontalLayoutGroup> _cellsShelterInventory = new List<HorizontalLayoutGroup>();
    private List<HorizontalLayoutGroup> _cellsShelterStorage = new List<HorizontalLayoutGroup>();

    private bool _isShelterOpened;

    private void OnEnable()
    {
        if (manager == null)
            manager = this;
        else if (manager == this) Destroy(gameObject);

        //OnCharacterLinked += SetInventoryEquiped;

        _isShelterOpened = false;
    }

    private void Start()
    {
        _cellsInventory = _gridLayoutTabInventory.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();

        _cellsShelterInventory = _gridLayoutShelterInventory.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
        _cellsShelterStorage = _gridLayoutShelterStorage.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
    }

    protected void LoadInventoryShelter()
    {
        var savedData = PlayerPrefs.GetString("shelterStorage", "").Trim();
        if (savedData.Length == 0) return;
        var listOfItems = savedData.Split(' ');
        List<int> items = new List<int>();
        for (int i = 0; i < listOfItems.Length; i++)
        {
            items.Add(Convert.ToInt32(listOfItems[i]));
        }
        for (int i = 0; i < items.Count; i++)
        {
            var data = ItemDataManager.itemManager.GetItemDataByIndex(items[i]);
            AddItem(data, _cellsShelterStorage).SetShelter();
        }
    }

    public void SaveShelter()
    {
        string saveString = "";
        for (int i = 0; i < _cellsShelterStorage.Count; i++)
        {
            var item = _cellsShelterStorage[i].GetComponentInChildren<ItemEquipable>();
            if (item == null)
            {
                saveString=  saveString.Trim();
                break;
            }

            saveString += ItemDataManager.itemManager.GetIndexByItemData(item.GetItemData()).ToString() + " ";
        }
        PlayerPrefs.SetString("shelterStorage", saveString);

    }

    public void SaveInventoryDouble() 
    {
        string saveString = "";
        for (int i = 0; i < _cellsShelterInventory.Count; i++)
        {
            var item = _cellsShelterInventory[i].GetComponentInChildren<ItemEquipable>();
            if (item == null)
            {
                saveString = saveString.Trim();
                break;
            }

            saveString += ItemDataManager.itemManager.GetIndexByItemData(item.GetItemData()).ToString() + " ";
        }

        PlayerPrefs.SetString("inventoryMain", saveString);

        SaveGun();
    }

    public void MoveToShelter(ItemData item, GameObject itemGameObject)
    {
        var shelterItem = SpawnEmptyItem(_cellsShelterStorage);
        shelterItem.SetItemBy(item, this);
        shelterItem.SetShelter();
        Destroy(itemGameObject);
    }

    public void MoveBackFromShelter(ItemData item, GameObject itemGameObject)
    {
        var shelterItem = SpawnEmptyItem(_cellsShelterInventory);
        shelterItem.SetItemBy(item, this);
        shelterItem.SetBoolFromShelterToInventory();
        Destroy(itemGameObject);
    }


    public void ShelterOpened()
    {
        LoadInventoryShelter();
        LoadInventory(_cellsShelterInventory);

        _isShelterOpened = true;
    }

    public bool ShelterActiveSelf()
    {
        return _isShelterOpened;
    }

    public void DeleteShelterStorage()
    {
        _isShelterOpened = false;
        DeleteInventory(_gridLayoutShelterInventory);
        DeleteInventory(_gridLayoutShelterStorage);
    }

    private void OnDisable()
    {
      //  OnCharacterLinked -= SetInventoryEquiped;
    }

}
