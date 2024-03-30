using TMPro;
using UnityEngine;
using Zenject;
public class WeaponController : MonoBehaviour
{
    //[SerializeField]
    private WeaponFactory _weaponFactory;

    [SerializeField] private AimRigController _rigController;
    [SerializeField] private TextMeshProUGUI _weaponUI;

    private Weapon _currentWeapon;
    public Weapon CurrentWeapon => _currentWeapon;

    [Inject]
    private void Construct(WeaponFactory weaponFactory)
    {
        Debug.Log("WeaponConstruct: " + weaponFactory + "\nName: " + gameObject.name);
        _weaponFactory = weaponFactory;
    }

    public void ChangeWeapon(WeaponData data, Transform holder)
    {
        if (_currentWeapon && data)
        {
            print(data + " ‰‡Ú‡ ‡‡‡‡");
            if (_currentWeapon.WeaponName == data.WeaponName)
                return;
            Destroy(_currentWeapon.gameObject);
        }
        Debug.Log("WeaponFactory: " + _weaponFactory);
        _currentWeapon = _weaponFactory.CreateWeapon(data, holder);
        SetWeaponPlayerSettings();
    }

    private void SetWeaponPlayerSettings()
    {
        if (_weaponUI)
            _currentWeapon.SetWeaponUI(_weaponUI);

        SetWeaponAimRig();
    }

    private void SetWeaponAimRig()
    {
        switch (_currentWeapon.WeaponSubType)
        {
            case WeaponSubType.Pistol:
                _rigController.SetCurrentRig(0);
                break;

            case WeaponSubType.Rifle:
                _rigController.SetCurrentRig(1);
                break;

            case WeaponSubType.Shotgun:
                _rigController.SetCurrentRig(1);
                break;

            case WeaponSubType.MachineGun:
                _rigController.SetCurrentRig(1);
                break;

            case WeaponSubType.Sword:
                _rigController.SetCurrentRig(2);
                break;
        }
    }


}
