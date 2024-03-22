using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;

public class EnemyLoot : MonoBehaviour
{
    public List<ItemData> items;
    public List<float> dropChances = new List<float> { 0.2f, 0.3f, 0.4f };//10% шанс ничего не заспавнить
    public int amountToDrop = 1;

    public List<InventoryItemManager.itemRarity> possibleQualities = new List<InventoryItemManager.itemRarity> { InventoryItemManager.itemRarity.gray, InventoryItemManager.itemRarity.green, InventoryItemManager.itemRarity.purple };
    public List<float> qualityChances = new List<float> { 0.8f, 0.15f, 0.05f }; // шанс качества предмета

    public void DropItems()
    {
        for (int i = 0; i < amountToDrop; i++)
        {
            ItemData itemToDrop = GetRandomItem();
            if (itemToDrop != null)
            {
                itemToDrop.rarity = GetRandomQuality();
                InventoryItemManager.manager.AddItem(itemToDrop);
            }
        }
    }

    private ItemData GetRandomItem()
    {
        float totalChance = 0;
        foreach (float chance in dropChances)
        {
            totalChance += chance;
        }

        float randomPoint = Random.value * totalChance;//

        for (int i = 0; i < items.Count; i++)
        {
            if (randomPoint < dropChances[i])
            {
                return items[i];
            }
            else
            {
                randomPoint -= dropChances[i];
            }
        }
        return null;
    }

    private InventoryItemManager.itemRarity GetRandomQuality()
    {
        float totalChance = 0;
        foreach (float chance in qualityChances)
        {
            totalChance += chance;
        }

        float randomPoint = Random.value * totalChance;

        for (int i = 0; i < possibleQualities.Count; i++)
        {
            if (randomPoint < qualityChances[i])
            {
                return possibleQualities[i];
            }
            else
            {
                randomPoint -= qualityChances[i];
            }
        }
        return InventoryItemManager.itemRarity.gray;
    }
}