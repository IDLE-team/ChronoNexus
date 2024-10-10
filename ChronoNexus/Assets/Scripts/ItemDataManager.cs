using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager itemManager;
    [SerializeField] private List<ItemData> _allGameItems = new List<ItemData>();


    private void Awake()
    {
        if (!itemManager)
        {
            itemManager = this;
        }
        else if (itemManager == this)
        {
            Destroy(gameObject);
        }
    }


    public int GetIndexByItemData(ItemData item)
    {
        return _allGameItems.IndexOf(item);
    }

    public ItemData GetItemDataByIndex(int index)
    {
        return _allGameItems[index];
    }

    public List<ItemData> GetAllGameItems()
    {
        return _allGameItems;
    }

    public int GetItemListCount()
    {
        return _allGameItems.Count;
    }
}
