using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabData : MonoBehaviour
{
   [SerializeField] private MainButtonController _mainButtonController;
   [SerializeField] private InventoryItemManager _inventoryItemManager;
   [SerializeField] private WinScreen _winScreen;
   [SerializeField] private DeathScreen _deathScreen;

   public MainButtonController MainButtonController => _mainButtonController;
   public InventoryItemManager InventoryItemManager => _inventoryItemManager;
   public WinScreen WinScreen => _winScreen;
   public DeathScreen DeathScreen => _deathScreen;

}
