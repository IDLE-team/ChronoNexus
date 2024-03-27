using UnityEngine;

public class TabLoadInventory : MonoBehaviour
{

    [SerializeField] private HubIventoryManager inventoryManager;

    private void OnEnable()
    {
        inventoryManager.InventoryMainOpened();
    }

    private void OnDisable()
    {
        inventoryManager.SaveInventory();
        inventoryManager.DeleteInventoryStorage();
    }

}
