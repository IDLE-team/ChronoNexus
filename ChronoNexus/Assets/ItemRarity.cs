using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemRarity
{
    public int chance;
    public Color rarityColor;
    [Serializable]
    public enum ItemRarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    };
}
