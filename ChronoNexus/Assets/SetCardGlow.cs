using UnityEngine;
using UnityEngine.UI;
using static InventoryItemManager;

public class SetCardGlow : MonoBehaviour
{
    [SerializeField] private Material _gray;
    [SerializeField] private Material _green;
    [SerializeField] private Material _purple;
    [SerializeField] private Material _gold;
    [SerializeField] private Material _red;

    [SerializeField] private Image _image;
    public void SetGlowColor(itemRarity rarity)
    {
        switch (rarity)
        {
            case itemRarity.gray:
                _image.material = _gray;
                return;
            case itemRarity.green:
                _image.material = _green;
                return;
            case itemRarity.purple:
                _image.material = _purple;
                return;
            case itemRarity.gold:
                _image.material = _gold;
                return;
            case itemRarity.red:
                _image.material = _red;
                return;

        }
    }

}
