using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeWeaponTab : MonoBehaviour
{
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Image _circleRarity;

    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _weaponLevel;

    [SerializeField] private Transform _containerUpgrader;

    [SerializeField] private GameObject _equipCell;

    [SerializeField] private int damageStep;
    [SerializeField] private int ammoStep;
    [SerializeField] private float speedStep;

    [SerializeField] private Button _buttonCraft;


    private ItemData _itemData;

    private List<UpgradeWeaponParameter> _parameters = new List<UpgradeWeaponParameter>();

    private void Start()
    {
        LoadNewWeapon();

        PlayerProfileManager.profile.itemChanged += LoadNewWeapon;
        PlayerProfileManager.profile.materialChanged += CheckLevel;
        PlayerProfileManager.profile.materialChanged += ReloadParameters;
        PlayerProfileManager.profile.materialChanged += SetupWeapon;

    }

    private void LoadNewWeapon()
    {
        if (GetWeapon())
        {
            SetupWeapon();

            _parameters = _containerUpgrader.GetComponentsInChildren<UpgradeWeaponParameter>().ToList();
            ReloadParameters();
        }
        else
        {
            _buttonCraft.GetComponent<CraftButtonInventory>().DelayShut();
            _buttonCraft.interactable = false;
        }
    }

    private bool GetWeapon()
    {
        if (_equipCell.GetComponentInChildren<ItemEquipable>())
        {
            _buttonCraft.interactable = true;
            _itemData = _equipCell.GetComponentInChildren<ItemEquipable>().GetItemData();
            return true;
        }
        else
        {
            _buttonCraft.interactable = false;
            return false;
        }
    }

    private void SetupWeapon()
    {
        _weaponImage.sprite = _itemData.itemImageSprite;
        _circleRarity.color = HubIventoryManager.manager.GetColorByRarity(_itemData.rarity);

        _weaponLevel.text = _itemData.itemLvl.ToString();
        _weaponName.text = _itemData.itemName;
    }

    private void CheckLevel()
    {
        _weaponLevel.text = _itemData.itemLvl.ToString();
    }

    private void OnDestroy()
    {
        PlayerProfileManager.profile.materialChanged += SetupWeapon;
        PlayerProfileManager.profile.materialChanged -= ReloadParameters;
        PlayerProfileManager.profile.materialChanged -= CheckLevel;
        PlayerProfileManager.profile.itemChanged -= LoadNewWeapon;
    }

    private void ReloadParameters()
    {
        _parameters[0].SetUpgrader(_itemData, UpgradeWeaponParameter.WeaponParameters.damage, damageStep);
        _parameters[1].SetUpgrader(_itemData, UpgradeWeaponParameter.WeaponParameters.fireRate, speedStep);
        _parameters[2].SetUpgrader(_itemData, UpgradeWeaponParameter.WeaponParameters.ammo, ammoStep);
    }




}
