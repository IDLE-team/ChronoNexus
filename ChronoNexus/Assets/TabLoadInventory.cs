using UnityEngine;

public class TabLoadInventory : MonoBehaviour
{

    [SerializeField] private InventoryItemManager inventoryManager;

    private void OnEnable()
    {
        inventoryManager.LoadGun();
        inventoryManager.InventoryMainOpened();
    }


    private void OnDisable()
    {
        inventoryManager.SaveInventory();
        inventoryManager.SaveGun();
        
        //inventoryManager.LoadGun();
        
        inventoryManager.DeleteInventoryStorage();
        inventoryManager.DeleteEquiped();
    }

}
