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

    [Header("���� ���������")]
    public WeaponData weaponData;

    // [Header("���� �������� ������")]
    //public KnifeData knifeData;

    //[Header("���� �������")]
    //public GranadeData granadeData;

    //[Header("���� �����")]
    //public ArmorData armorData;

}