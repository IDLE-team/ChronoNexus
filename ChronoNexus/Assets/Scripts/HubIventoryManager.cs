using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HubIventoryManager : InventoryItemManager
{
    public static HubIventoryManager manager;

    [SerializeField] private GameObject _chestOpenUI;
    private bool _isShelterOpened;

    private void OnEnable()
    {
        if (manager == null)
            manager = this;
        else if (manager == this) Destroy(gameObject);
        _isShelterOpened = false;
    }

    private void Start()
    {
        _cellsInventory = _gridLayoutTabInventory.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();

        var savedData = PlayerPrefs.GetInt("gun", -1);
        if (savedData == -1)
        {
            PlayerPrefs.SetInt("gun", 2);
            savedData = PlayerPrefs.GetInt("gun", 2);
        };
    }

    private void Update()
    {
        SortInventory();
    }


    public GameObject GetChestOpenUI()
    {
        return _chestOpenUI;
    }


    private void OnDisable()
    {
        //  OnCharacterLinked -= SetInventoryEquiped;
    }

}
