using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabData : MonoBehaviour
{
   [SerializeField] private MainButtonController _mainButtonController;
   [SerializeField] private InventoryItemManager _inventoryItemManager;

   public MainButtonController MainButtonController => _mainButtonController;
   public InventoryItemManager InventoryItemManager => _inventoryItemManager;

}
