using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinCell : MonoBehaviour
{
    [SerializeField] private Button _buttonTake;
    [SerializeField] private Button _buttonTaken;
    [SerializeField] private Button _buttonBuy;

    [SerializeField] private TextMeshProUGUI _textCost;
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textDescription;

    [SerializeField] private Image _imageHero;
    [SerializeField] private Image _imageSkill;

    [SerializeField] private bool isGift = false;
    [SerializeField] private SetCardGlow _rarityImage;

    private SkinData _data;
    private int _indexSkin;

    private void Start()
    {
        if (!isGift)
        {
            PlayerProfileManager.profile.heroChanged += CheckSkinTaken;
            _buttonTake.onClick.AddListener(TakeSkin);
            _buttonBuy.onClick.AddListener(BuySkin);
        }

    }

    public void SetSkinCell(SkinData skinData)
    {
        _data = skinData;
        _indexSkin = SkinDataManager.skinManager.GetIndexBySkinData(_data);
        _textCost.text = skinData.charCost.ToString();
        _textName.text = skinData.charName.ToString();
        _textDescription.text = skinData.charDescription.ToString();

        _imageHero.sprite = skinData.charSprite;
        _imageSkill.sprite = skinData.mainSkill;

        _rarityImage.SetGlowColor(skinData.rarity);
        CheckSkinTaken();
    }

    public void SetSkinCellGift(SkinData skinData)
    {
        if (isGift)
        {
            _data = skinData;
            _indexSkin = SkinDataManager.skinManager.GetIndexBySkinData(_data);

            _textName.text = skinData.charName.ToString();
            _textDescription.text = skinData.charDescription.ToString();

            _imageHero.sprite = skinData.charSprite;

            _buttonTake.onClick.AddListener(GetGiftSkin);
            _buttonTaken.onClick.AddListener(TakeGiftSkin);

            _rarityImage.SetGlowColor(skinData.rarity);
        }
    }

    private void GetGiftSkin()
    {
        _data.isBought = true;
        CloseWindowGift();
    }
    private void TakeGiftSkin()
    {
        PlayerPrefs.SetInt("hero", _indexSkin);
        PlayerProfileManager.profile.OnHeroChanged();
        CloseWindowGift();
    }

    private void CloseWindowGift()
    {
        gameObject.GetComponentInParent<GiftDropTab>().CloseWindow();
    }

    private void TakeSkin()
    {
        PlayerPrefs.SetInt("hero", _indexSkin);
        _buttonTaken.gameObject.SetActive(true);
        _buttonTake.gameObject.SetActive(false);
        _buttonBuy.gameObject.SetActive(false);
        PlayerProfileManager.profile.OnHeroChanged();
    }

    private void BuySkin()
    {
        HubIventoryManager.manager.GetMoneyHolder().DecreaseMoneyValue(_data.charCost);
        PlayerProfileManager.profile.OnMoneyChange();
        _data.isBought = true;

        _buttonTake.gameObject.SetActive(true);
        _buttonTaken.gameObject.SetActive(false);
        _buttonBuy.gameObject.SetActive(false);

        GiftDropTab.instance.SetGift(_data);
    }



    private void CheckSkinTaken()
    {
        if (PlayerPrefs.GetInt("hero") == SkinDataManager.skinManager.GetIndexBySkinData(_data))
        {
            _buttonTaken.gameObject.SetActive(true);
            _buttonTake.gameObject.SetActive(false);
            _buttonBuy.gameObject.SetActive(false);
        }
        else
        {
            if (_data.isBought)
            {
                _buttonTake.gameObject.SetActive(true);
                _buttonTaken.gameObject.SetActive(false);
                _buttonBuy.gameObject.SetActive(false);
            }
            else
            {
                _buttonBuy.gameObject.SetActive(true);
                if (HubIventoryManager.manager.GetMoneyValue() >= _data.charCost)
                {
                    _buttonBuy.interactable = true;
                }
                else
                {
                    _buttonBuy.interactable = false;
                }
                _buttonTaken.gameObject.SetActive(false);
                _buttonTake.gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        if (_buttonTaken.gameObject.activeSelf)
        {
            PlayerPrefs.SetInt("hero", SkinDataManager.skinManager.GetIndexBySkinData(_data));
        }
    }
}
