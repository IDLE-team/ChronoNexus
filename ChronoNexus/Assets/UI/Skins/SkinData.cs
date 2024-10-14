using UnityEngine;
using static InventoryItemManager;


[CreateAssetMenu(fileName = "SkinData", menuName = "SkinHolder")]
public class SkinData : ScriptableObject
{
    public Sprite charSprite;
    public Sprite mainSkill;
    public string charName;
    public string charDescription;
    public bool isBought;
    public int charCost;
    public int damageBase;
    public int hpBase;
    public int speedBase;
    public int skillID;
    public itemRarity rarity;
}

