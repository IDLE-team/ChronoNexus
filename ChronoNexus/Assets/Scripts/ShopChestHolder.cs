using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ShopChestHolder : MonoBehaviour
{

    [SerializeField] private InventoryItemManager.itemRarity _itemRarity;
    [SerializeField] private int _itemsAmount;
    [SerializeField] private Sprite _chestSprite;


    [Header("Кнопка продажи и цена")]
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private int _itemCost;
    [SerializeField] private Button _purchaseButton;
    [SerializeField] private ChestOpenTab _lootSetter;

    
    // Часть под сундуки
    [Header ("Доступные предметы в сундуке")]

    [SerializeField] bool _isGun = true;
    [SerializeField] bool _isMoney;
    [SerializeField] bool _isMaterial;
    [SerializeField] bool _isXp;

    

    [SerializeField] private GameObject _itemGrid;
    [SerializeField] private GameObject _itemBlank;

    [SerializeField] private Sprite _itemGun;
    [SerializeField] private Sprite _itemMoney;
    [SerializeField] private Sprite _itemMaterial;
    [SerializeField] private Sprite _itemXp;

    // конец части под сундуки

    private void OnDrawGizmos()
    {
        _costText.text = _itemCost.ToString();

        _itemGrid = GetComponentInChildren<GridLayoutGroup>().gameObject;

       
    }

    private void Start()
    {
        #region SetItemBysIcon
        if (_isGun)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = _itemGun;
        }
        /*
        if (_isKnife)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = IconsToBuy[1];
        }
        if (_isGranade)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = IconsToBuy[2];
        }
        if (_isArmor)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = IconsToBuy[3];
        }
        */
        if (_isMoney)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = _itemMoney;
        }
        if (_isMaterial)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = _itemMaterial;
        }
        if (_isXp)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = _itemXp;
        }
        #endregion
        _costText.text = _itemCost.ToString();

        PlayerProfileManager.profile.moneyChanged += PurchaseButtonActive;

        _purchaseButton.onClick.AddListener(PurchaseChest);

        PurchaseButtonActive();
    }

    private void PurchaseButtonActive()
    {
        if (HubIventoryManager.manager.GetMoneyValue() >= _itemCost)
        {
            _purchaseButton.interactable = true;
        }
        else
        {
            _purchaseButton.interactable = false;
        }
    }

    private void PurchaseChest()
    {
        if (HubIventoryManager.manager.BuyItem(_itemCost))
        {
            HubIventoryManager.manager.GetChestOpenUI().SetActive(true);

            _lootSetter.SetInfoChest(_itemRarity, _itemsAmount, _chestSprite);

            PlayerProfileManager.profile.OnMoneyChange();
        }
    }

    private void OnDestroy()
    {
        PlayerProfileManager.profile.moneyChanged -= PurchaseButtonActive;
    }

}
