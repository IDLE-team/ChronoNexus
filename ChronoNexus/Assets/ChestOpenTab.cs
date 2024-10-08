using UnityEngine;
using UnityEngine.UI;

public class ChestOpenTab : MonoBehaviour
{
    [SerializeField] private Image _chestImage;

    [SerializeField] private GameObject _chestOpenTab;
    [SerializeField] private GameObject _lootOpenTab;

    [SerializeField] private GetLootFromChest _chestContainer;

    private void OnEnable()
    {
        _chestOpenTab.SetActive(true);
        _lootOpenTab.SetActive(false);
    }


    public void SetInfoChest(HubIventoryManager.itemRarity rarity, int amountOfItems, Sprite chestSprite)
    {
        _chestContainer.SetInfoChest(rarity, amountOfItems);

        _chestImage.sprite = chestSprite;
    }

    private void OnDisable()
    {
        _chestOpenTab.SetActive(true);
        _lootOpenTab.SetActive(false);
    }
}
