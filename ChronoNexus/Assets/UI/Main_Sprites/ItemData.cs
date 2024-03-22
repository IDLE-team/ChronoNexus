using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Items/ItemData", order = 1)]
public class ItemData : ScriptableObject
{

    public string itemName;
    public float itemCost;
    public InventoryItemManager.itemRarity rarity;
    public InventoryItemManager.itemType itemType;
    public int itemLvl;
    public Sprite itemImageSprite;

    [Header("Если огнестрел")]
    public WeaponData weaponData;

    // [Header("Если холодное оружие")]
    //public KnifeData knifeData;

    //[Header("Если граната")]
    //public GranadeData granadeData;

    //[Header("Если броня")]
    //public ArmorData armorData;

}