using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HubIventoryManager : InventoryItemManager
{

    [Header("Хранилища для хаба")]
    [SerializeField] private GameObject _gridLayoutShelterInventory;
    [SerializeField] private GameObject _gridLayoutShelterStorage;

    private List<HorizontalLayoutGroup> _cellsShelterInventory = new List<HorizontalLayoutGroup>();
    private List<HorizontalLayoutGroup> _cellsShelterStorage = new List<HorizontalLayoutGroup>();

    private void OnEnable()
    {
        OnCharacterLinked += SetInventoryEquiped;
    }

    private void Start()
    {
        _cellsInventory = _gridLayoutTabInventory.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();

        _cellsShelterInventory = _gridLayoutShelterInventory.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();
        _cellsShelterStorage = _gridLayoutShelterStorage.GetComponentsInChildren<HorizontalLayoutGroup>().ToList();


        
        OnInventoryChanged += SaveInventory;

        LoadInventory();
    }

    private void OnDisable()
    {
        OnCharacterLinked -= SetInventoryEquiped;
        OnInventoryChanged();
        OnInventoryChanged -= SaveInventory;
    }
}
