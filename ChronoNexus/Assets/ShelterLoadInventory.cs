using UnityEngine;

public class ShelterLoadInventory : MonoBehaviour
{

    [SerializeField] private HubIventoryManager inventoryManager;

    private void OnEnable()
    {
        inventoryManager.ShelterOpened();
        inventoryManager.LoadGun();
    }

    private void OnDisable()
    {
        inventoryManager.SaveInventoryDouble();
        inventoryManager.SaveShelter();
        inventoryManager.SaveGun();
        inventoryManager.DeleteShelterStorage();
        inventoryManager.DeleteEquiped();
    }

}
