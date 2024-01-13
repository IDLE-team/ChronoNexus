using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyLoot : MonoBehaviour
{
    public List<Item> lootTable;

    public void DropLoot()
    {
        if (lootTable.Count == 0)
        {
            Debug.LogWarning("Loot table is empty.");
            return;
        }
        Item droppedItem = ChooseRandomItem();
        if (droppedItem != null)
        {
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cube = Instantiate(droppedItem.prefab, transform);
            cube.GetComponent<Renderer>().material.color = droppedItem.rarity.rarityColor;
        }
        else
        {
            Debug.LogWarning("Failed to drop loot.");
        }
    }

    private Item ChooseRandomItem()
    {
        float totalChance = 0;
        foreach (var item in lootTable)
        {
            totalChance += item.chanceToDropItem;
        }

        float randomValue = Random.Range(0, totalChance);

        foreach (var item in lootTable)
        {
            if (randomValue < item.chanceToDropItem)
            {
                return item;
            }
            randomValue -= item.chanceToDropItem;
        }

        return null;
    }
    
}

