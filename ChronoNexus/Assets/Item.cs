using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public GameObject prefab;
    public string name;
    public ItemRarity rarity;
    public ItemRarity.ItemRarityType[] possibleRarityItemTypes;
    public float chanceToDropItem;

}
