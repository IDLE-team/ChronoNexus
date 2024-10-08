using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GetLootFromChest : MonoBehaviour
{

    [SerializeField] private GameObject _lootCardPrefab;

    [SerializeField] private Transform _centerPoint;
    [SerializeField] private Transform _previousPoint;
    [SerializeField] private Transform _lootEndHolder;

    [SerializeField] private int _cardStepSize = -350;

    [SerializeField] private GameObject _chestOpenUI;

    private int cardsGranted = 0;

    private HubIventoryManager.itemRarity _rarity;
    private int _itemsAmount;

    private GameObject _prevCard = null;

    private List<GameObject> _grantedCardsObjects = new List<GameObject>();
    private bool _cardsRevealed = false;

    private void OnEnable()
    {
        GrantCard();
    }

    public void GrantCard()
    {
        if (cardsGranted >= _itemsAmount)
        {
            if (!_cardsRevealed)
            {
                _cardsRevealed = true;
                for (int i = 0; i < _grantedCardsObjects.Count; i++)
                {
                    var rect = _grantedCardsObjects[i].GetComponent<RectTransform>();

                    _grantedCardsObjects[i].GetComponent<RectTransform>().position = new Vector2(Screen.width / 2, Screen.height / 2);

                    rect.localScale = Vector3.zero;

                    rect.DOMoveX(_grantedCardsObjects[i].GetComponent<RectTransform>().position.x + (((float)_grantedCardsObjects.Count / 2 + 0.5f) - (i + 1)) * _cardStepSize, 0.2f);
                    rect.DOScale(Vector3.one, 0.6f);

                }
                foreach (var item in _grantedCardsObjects)
                {
                    item.GetComponent<RectTransform>().SetParent(_lootEndHolder);
                }

            }
            else
            {
                foreach (var item in _grantedCardsObjects)
                {
                    Destroy(item);
                }
                _grantedCardsObjects.Clear();
                _chestOpenUI.transform.DOScale(Vector3.zero, 0.4f);
                _chestOpenUI.transform.localScale = Vector3.one;
                _chestOpenUI.SetActive(false);
            }
        }
        else
        {
            if (_prevCard)
            {
                _prevCard.transform.DOMove(_previousPoint.transform.position, 0.3f).SetEase(Ease.InSine);
            }
            var card = GenerateItem(_rarity);
            _grantedCardsObjects.Add(card);

            card.transform.localScale = Vector3.zero;
            card.transform.DOScale(Vector3.one, 0.3f);

            cardsGranted++;

            _prevCard = card;

        }
    }

    public void SetInfoChest(HubIventoryManager.itemRarity rarity, int amountOfItems)
    {
        _rarity = rarity;
        _itemsAmount = amountOfItems;
    }

    /// <summary>
    /// Только для расходников - деньги, материалы, опыт
    /// </summary>
    private GameObject CreateCard(HubIventoryManager.lootType lootType, int Amount)
    {
        var g = Instantiate(_lootCardPrefab, _centerPoint.transform);
        g.transform.position = _centerPoint.transform.position;
        g.GetComponent<LootCard>().DisplayLootData(lootType, Amount);
        return g;
    }

    /// <summary>
    /// Только для оружия, выбор редкости
    /// </summary>
    private GameObject CreateCard(HubIventoryManager.itemRarity rarity)
    {
        var g = Instantiate(_lootCardPrefab, _centerPoint.transform);
        g.transform.position = _centerPoint.transform.position;
        g.GetComponent<LootCard>().DisplayLootData(rarity);
        return g;
    }



    private GameObject GenerateItem(HubIventoryManager.itemRarity chestRarity)
    {
        switch (chestRarity)
        {
            case HubIventoryManager.itemRarity.gray:

                switch (Random.Range(0, 1000))
                {
                    case >= 0 and <= 250: // money 25%
                        return CreateCard(HubIventoryManager.lootType.money, (Random.Range(100, 500) / 10) * 10);
                    case > 250 and <= 1000: // material 75%
                        return CreateCard(HubIventoryManager.lootType.material, (Random.Range(10, 30) / 5) * 5);

                }
                break;

            case HubIventoryManager.itemRarity.green:
                switch (Random.Range(0, 1000))
                {
                    case >= 0 and <= 250: // money 25%
                        return CreateCard(HubIventoryManager.lootType.money, (Random.Range(250, 750) / 50) * 50);
                    case > 250 and <= 500: // material 25%
                        return CreateCard(HubIventoryManager.lootType.material, (Random.Range(20, 50) / 5) * 5);
                    case > 500 and <= 1000: // gun 50%
                        return CreateCard((HubIventoryManager.itemRarity)Random.Range(0, 2));

                }
                break;

            case HubIventoryManager.itemRarity.purple:

                switch (Random.Range(0, 1000))
                {
                    case >= 0 and <= 150: // money 15%
                        return CreateCard(HubIventoryManager.lootType.money, (Random.Range(500, 1000) / 50) * 50);
                    case > 150 and <= 350: // material 20%
                        return CreateCard(HubIventoryManager.lootType.material, (Random.Range(30, 80) / 5) * 5);
                    case > 350 and <= 500: // exp 15%
                        return CreateCard(HubIventoryManager.lootType.exp, Random.Range(20, 40));
                    case > 500 and <= 1000: // gun 50%
                        return CreateCard((HubIventoryManager.itemRarity)Random.Range(1, 3));

                }

                break;

            case HubIventoryManager.itemRarity.gold:

                switch (Random.Range(0, 1000))
                {
                    case >= 0 and <= 150: // money 15%
                        return CreateCard(HubIventoryManager.lootType.money, (Random.Range(1000, 1500) / 50) * 50);
                    case > 150 and <= 250: // material 10%
                        return CreateCard(HubIventoryManager.lootType.material, (Random.Range(70, 100) / 5) * 5);
                    case > 250 and <= 350: // exp 10%
                        return CreateCard(HubIventoryManager.lootType.exp, Random.Range(50, 70));
                    case > 350 and <= 1000: // gun 65%
                        return CreateCard((HubIventoryManager.itemRarity)Random.Range(2, 4));

                }

                break;
        }

        return null;
    }

    private void OnDisable()
    {
        _prevCard = null;
        _cardsRevealed = false;
        cardsGranted = 0;
    }
}
