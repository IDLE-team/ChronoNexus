using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ShopChestHolder : MonoBehaviour
{
    public List<Sprite> IconsToBuy = new List<Sprite>();

    [Header("Кнопка продажи и цена")]
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private float _itemCost;
    [SerializeField] private Button _purchaseButton;

    
    // Часть под сундуки
    [Header ("Доступные предметы в сундуке")]

    [SerializeField] bool _isGun = true;
    [SerializeField] bool _isKnife;
    [SerializeField] bool _isGranade = true;
    [SerializeField] bool _isArmor = true;
    [SerializeField] bool _isMoney;

    [SerializeField] private GameObject _itemGrid;
    [SerializeField] private GameObject _itemBlank;

    private List<Image> _items = new List<Image>();
    // конец части под сундуки

    private void OnDrawGizmos()
    {
        if (!_costText)
        {
            _costText = GetComponentInChildren<TextMeshProUGUI>();
        }
        _costText.text = _itemCost.ToString();

        _itemGrid = GetComponentInChildren<GridLayoutGroup>().gameObject;
    }

    private void Start()
    {
        _items = _itemGrid.GetComponentsInChildren<Image>().ToList();
        foreach (Image item in _items)
        {
            Destroy(item.gameObject);
        }
        #region setItemsIcon
        if (_isGun)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = IconsToBuy[0];
        }
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
        if (_isMoney)
        {
            GameObject g = Instantiate(_itemBlank, _itemGrid.transform);
            g.GetComponent<Image>().sprite = IconsToBuy[4];
        }
        #endregion
    }

}
