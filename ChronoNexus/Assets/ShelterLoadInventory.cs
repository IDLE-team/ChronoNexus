using UnityEngine;

public class ShelterLoadInventory : MonoBehaviour
{

    [SerializeField] private HubIventoryManager inventoryManager;

    private void OnEnable()
    {
        inventoryManager.ShelterOpened();
    }

    private void OnDisable()
    {
        inventoryManager.SaveInventoryDouble();
        inventoryManager.SaveShelter();
        inventoryManager.DeleteShelterStorage();
    }

}
