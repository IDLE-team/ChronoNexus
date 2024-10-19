using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponTab : MonoBehaviour
{
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Image _circleRarity;

    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private TextMeshProUGUI _weaponLevel;
    [SerializeField] private TextMeshProUGUI _weaponType;

    [SerializeField] private Transform _containerUpgrader;

    [SerializeField] private GameObject _equipCell;

    [SerializeField] private int damageStep;
    [SerializeField] private int ammoStep;
    [SerializeField] private float speedStep;

    [SerializeField] private Button _buttonCraft;

    [SerializeField] private Button _buttonSell;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _buttonMaterial;
    [SerializeField] private TextMeshProUGUI _materialText;


    private ItemData _itemData;

    private List<UpgradeWeaponParameter> _parameters = new List<UpgradeWeaponParameter>();

    private void Start()
    {
        LoadNewWeapon();

        PlayerProfileManager.profile.itemChanged += LoadNewWeapon;
        PlayerProfileManager.profile.materialChanged += CheckLevel;
        PlayerProfileManager.profile.materialChanged += ReloadParameters;
        PlayerProfileManager.profile.materialChanged += SetupWeapon;

        _buttonSell.onClick.AddListener(SellWeapon);
        _buttonMaterial.onClick.AddListener(MaterializeWeapon);

    }

    private void SellWeapon()
    {

        if (_itemData != null)
        {
            HubIventoryManager.manager.GetMoneyHolder().IncreaseMoney(_itemData.itemCost);
            PlayerProfileManager.profile.moneyChanged();
            DestructItem();
        }

    }

    private void MaterializeWeapon()
    {
        if (_itemData != null)
        {
            HubIventoryManager.manager.GetMaterialHolder().IncreaseMaterialValue(_itemData.itemLvl * 3);
            PlayerProfileManager.profile.materialChanged();
            DestructItem();
        }
    }

    private void DestructItem()
    {
        Destroy(_equipCell.GetComponentInChildren<ItemEquipable>().gameObject);
        _itemData = null;

        PlayerProfileManager.profile.itemChanged();
        _buttonCraft.GetComponent<CraftButtonInventory>().DelayShut();
        _buttonCraft.interactable = false;
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
        _weaponName.text = _itemData.weaponData.WeaponName;
        _weaponType.text = _itemData.itemName;

        if (HubIventoryManager.manager.CountItems() == 0)
        {
            _costText.text = "Õ≈À‹«ﬂ œ–Œƒ¿“‹ œŒ—À≈ƒÕ≈≈ Œ–”∆»≈";
            _materialText.text = "Õ≈À‹«ﬂ –¿«Œ¡–¿“‹ œŒ—À≈ƒÕ≈≈ Œ–”∆»≈";
            _buttonSell.interactable = false;
            _buttonMaterial.interactable = false;
        }
        else
        {
            _buttonSell.interactable = true;
            _buttonMaterial.interactable = true;
            _costText.text = _itemData.itemCost.ToString();
            _materialText.text = (_itemData.itemLvl * 3).ToString();
        }
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
