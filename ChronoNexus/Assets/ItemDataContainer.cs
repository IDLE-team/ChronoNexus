using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class ItemDataContainer : MonoBehaviour
{
    public ItemDataContainer(ItemData itemData)
    {
        _itemData = itemData;
    }
    public ItemData _itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.DOMove(other.transform.position, 0.2f).OnComplete(() =>
            {
                other.GetComponent<Character>().InventoryItemManager.AddItem(_itemData);
                Destroy(gameObject);
            });
        }
    }
}
