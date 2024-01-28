using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;

public class EnemyLoot : MonoBehaviour
{
    

    [Serializable]
    public class Item
    {
        public string name;
        public Rarity rarity;
        [Range(0, 100)]
        public int dropChance;
        public GameObject prefab;
        public Color GetRarityColor()
        {
            switch (rarity)
            {
                case Rarity.Common:
                    return Color.gray;
                case Rarity.Uncommon:
                    return Color.blue;
                case Rarity.Rare:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }
        public enum Rarity
        {
            Common,
            Uncommon,
            Rare
        }

    }

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
            Debug.Log("Dropped item: " + droppedItem.name);

            GameObject prefabItem = Instantiate(droppedItem.prefab, transform.position, Quaternion.identity,transform.parent);
            prefabItem.GetComponent<Renderer>().material.color = droppedItem.GetRarityColor();
        }
        else
        {
            Debug.LogWarning("Failed to drop loot.");
        }
    }

    private Item ChooseRandomItem()
    {
        int totalChance = lootTable.Sum(item => item.dropChance);
        int randomValue = Random.Range(0, totalChance);

        foreach (var item in lootTable)
        {
            if (randomValue < item.dropChance)
            {
                return item;
            }
            randomValue -= item.dropChance;
        }

        return null;
    }
    
}