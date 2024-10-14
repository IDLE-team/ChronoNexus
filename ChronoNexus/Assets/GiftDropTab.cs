using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftDropTab : MonoBehaviour
{
    public static GiftDropTab instance;

    [SerializeField] private LootCard _lootCard;
    [SerializeField] private SkinCell _skinCard;

    [SerializeField] private Transform _UIHolder;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(this);
        }
    }

    public void SetGift(SkinData skinData)
    {
        _UIHolder.localScale = Vector3.zero;
        _UIHolder.DOScale(Vector3.one, 0.2f);

        _skinCard.gameObject.SetActive(true);
        _lootCard.gameObject.SetActive(false);

        _skinCard.SetSkinCellGift(skinData);
    }
    public void SetGift(ItemData item)
    {
        _UIHolder.localScale = Vector3.zero;
        _UIHolder.DOScale(Vector3.one, 0.2f);

        _skinCard.gameObject.SetActive(false);
        _lootCard.gameObject.SetActive(true);

        _lootCard.DisplayLootData(item);
    }

    public void CloseWindow()
    {
        _UIHolder.DOScale(Vector3.zero, 0.2f);
    }
}
