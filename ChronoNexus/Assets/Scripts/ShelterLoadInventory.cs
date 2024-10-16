using UnityEngine;

public class ShelterLoadInventory : MonoBehaviour
{

    [SerializeField] private HubIventoryManager inventoryManager;

    private void OnEnable()
    {
        inventoryManager.LoadGun();
    }

    private void OnDisable()
    {
        inventoryManager.SaveGun();
        inventoryManager.DeleteEquiped();
    }

}
