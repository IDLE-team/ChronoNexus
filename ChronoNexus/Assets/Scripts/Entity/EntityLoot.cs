using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryItemManager;

public class EntityLoot : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject ItemPrefab;
    public List<ItemData> items;
    public List<float> dropChances = new List<float> { 0.2f, 0.3f, 0.4f };
    public int amountToDrop = 1;

   public List<InventoryItemManager.itemRarity> possibleQualities = new List<InventoryItemManager.itemRarity>
   {
       InventoryItemManager.itemRarity.gray, InventoryItemManager.itemRarity.green, InventoryItemManager.itemRarity.purple
   };
    public List<float> qualityChances = new List<float> { 0.8f, 0.15f, 0.5f }; 

    public void DropItems()
    {
        if (ItemPrefab == null || items.Count == 0 || dropChances.Count == 0||amountToDrop==0||possibleQualities.Count==0||qualityChances.Count==0) 
        {
            return;
        }
        for (int i = 0; i < amountToDrop; i++)
        { 
            ItemData itemToDrop = GetRandomItem(GetRandomQuality());
            
            if (itemToDrop != null)
            {
                GameObject itemObj = Instantiate(ItemPrefab,spawnPosition.position,Quaternion.identity);
                 
                ItemDataContainer _itemDataContainer = itemObj.GetComponent<ItemDataContainer>();
                _itemDataContainer._itemData = itemToDrop;
                _itemDataContainer._itemDataColorSet.SetColor(itemToDrop.rarity);
                
                Rigidbody rb = itemObj.GetComponent<Rigidbody>();
                Vector3 forceDir = Random.insideUnitSphere.normalized;
                rb.AddForce(forceDir * 30f, ForceMode.Impulse);
            }
        }
    }

    private ItemData GetRandomItem(itemRarity rarity)
    {

        List<ItemData> rarItems = new List<ItemData>();

        foreach (ItemData item in items)
        {
            if (item.rarity == rarity)
            {
                rarItems.Add(item);
            }
        }

        float totalChance = 0;
        foreach (float chance in dropChances)
        {
            totalChance += chance;
        } 
        float randomPoint = Random.Range(0,100) * totalChance; 
        if (randomPoint == 0)
        {
            return null;
        }
        for (int i = 0; i < rarItems.Count; i++)
        {
            if (randomPoint < dropChances[i]*100)
            {
                return rarItems[i];
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

        float randomPoint = Random.Range(0,100) * totalChance;

        for (int i = 0; i < 3; i++)
        {
            if (randomPoint < qualityChances[i]*100)
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
