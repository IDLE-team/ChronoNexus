using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using Zenject;
public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private WeaponFactory _weaponFactory;

    [SerializeField] private AimRigController _rigController;
    [SerializeField] private TextMeshProUGUI _weaponUI;
    [SerializeField] private GameObject _reloadUI;
    [SerializeField] private bool _isPlayer;
    private Weapon _currentWeapon;
    public Weapon CurrentWeapon => _currentWeapon;

    [Inject]
    private void Construct(WeaponFactory weaponFactory)
    {
        //Debug.Log("WeaponConstruct: " + weaponFactory + "\nName: " + gameObject.name);
        SetWeaponFactory(weaponFactory);
    }

   public void SetWeaponFactory(WeaponFactory weaponFactory)
    {
        _weaponFactory = weaponFactory;
    }
    public void ClearWeapon()
    {
        if(_currentWeapon != null)
            Destroy(_currentWeapon.gameObject);
    }
    public void ChangeWeapon(WeaponData data, Transform holder)
    {
        if (_currentWeapon && data)
        {
            if (_currentWeapon.WeaponName == data.WeaponName)
                return;
            Destroy(_currentWeapon.gameObject);
        }
        Debug.Log("WeaponFactory Factory: " + _weaponFactory);
        Debug.Log("WeaponFactory Data: " + data);
        Debug.Log("WeaponFactory Holder: " + holder);
        _currentWeapon = _weaponFactory.CreateWeapon(data, holder, _isPlayer);
        SetWeaponPlayerSettings();
    }

    private void SetWeaponPlayerSettings()
    {
        if (_weaponUI && _reloadUI)
            _currentWeapon.SetWeaponUI(_weaponUI, _reloadUI);
        if(_reloadUI)

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
            case WeaponSubType.Finisher:
                _rigController.SetCurrentRig(2);
                break;
        }
    }


}
