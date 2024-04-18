using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataColorSet : MonoBehaviour
{
    public Material GrayMaterial;
    public Material GreenMaterial;
    public Material PurpleMaterial;
    public Material GoldMaterial;
    public List<Renderer> Renderers;
    public void SetColor(InventoryItemManager.itemRarity rarity )
    {
        switch (rarity)
        {
            case InventoryItemManager.itemRarity.gray:
                SetMaterial(GrayMaterial);
                break;
            case InventoryItemManager.itemRarity.green:
                SetMaterial(GreenMaterial);
                break;
            case InventoryItemManager.itemRarity.purple:
                SetMaterial(PurpleMaterial);
                break;
            case InventoryItemManager.itemRarity.gold:
                SetMaterial(GoldMaterial);
                break;
        }
    }

    private void SetMaterial(Material material)
    {
        foreach (Renderer _renderer in Renderers)
        {
            _renderer.material = material;
        }
    }
}
