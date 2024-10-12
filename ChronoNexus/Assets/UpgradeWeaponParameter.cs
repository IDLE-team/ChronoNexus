using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponParameter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _parameterName;
    [SerializeField] private TextMeshProUGUI _paramCurrent;
    [SerializeField] private TextMeshProUGUI _paramUpgrade;
    [SerializeField] private TextMeshProUGUI _upgradeCost;

    [SerializeField] private Button _button;

    private int _cost;
    private float _step;
    private WeaponParameters _parameter;
    private ItemData _item;

    private void Start()
    {
        _button.onClick.AddListener(PurchaseUpgrade);

        PlayerProfileManager.profile.materialChanged += CheckCost;

    }
    public void SetUpgrader(ItemData item, WeaponParameters parameter, float step)
    {
        _item = item;
        _parameter = parameter;
        _step = step;

        switch (parameter)
        {
            case WeaponParameters.ammo:
                _parameterName.text = "Обойма";
                _paramCurrent.text = item.weaponData.MaxAmmo.ToString() + "->";

                _paramUpgrade.text = (item.weaponData.MaxAmmo + step).ToString();

                _upgradeCost.text = _cost.ToString();
                break;

            case WeaponParameters.fireRate:
                _parameterName.text = "Скорость атаки";
                _paramCurrent.text = item.weaponData.FireRate.ToString() + "->";

                _paramUpgrade.text = (item.weaponData.FireRate * (1 - step)).ToString();

                break;

            case WeaponParameters.damage:
                _parameterName.text = "Сила атаки";
                _paramCurrent.text = item.weaponData.Damage.ToString() + "->";

                _paramUpgrade.text = (item.weaponData.Damage + step).ToString();

                break;
        }

        _cost = item.itemLvl * 5;
        _upgradeCost.text = _cost.ToString();

        CheckCost();
    }

    public void PurchaseUpgrade()
    {
        switch (_parameter)
        {
            case WeaponParameters.ammo:

                if (HubIventoryManager.manager.GetMaterialHolder().DecreaseMaterialValue(_cost))
                {
                    _item.weaponData.MaxAmmo += (int)_step;
                    _item.itemLvl++;
                    PlayerProfileManager.profile.OnMaterialChanged();
                }

                break;

            case WeaponParameters.fireRate:
                if (HubIventoryManager.manager.GetMaterialHolder().DecreaseMaterialValue(_cost))
                {
                    _item.weaponData.FireRate = (float)System.Math.Round((double)_item.weaponData.FireRate + _step, 1);
                    _item.itemLvl++;
                    PlayerProfileManager.profile.OnMaterialChanged();
                }
                break;

            case WeaponParameters.damage:
                if (HubIventoryManager.manager.GetMaterialHolder().DecreaseMaterialValue(_cost))
                {
                    _item.weaponData.Damage += _step;
                    _item.itemLvl++;
                    PlayerProfileManager.profile.OnMaterialChanged();
                }
                break;
        }

        CheckCost();
        SetUpgrader(_item, _parameter, _step);
    }

    private void CheckCost()
    {
        if (_cost > HubIventoryManager.manager.GetMoneyValue())
        {
            _button.enabled = false;
        }
        else
        {
            _button.enabled = true;
        }

        _upgradeCost.text = _cost.ToString();
    }


    public enum WeaponParameters
    {
        ammo, damage, fireRate
    }

    private void OnDestroy()
    {
        PlayerProfileManager.profile.materialChanged -= CheckCost;
    }
}
