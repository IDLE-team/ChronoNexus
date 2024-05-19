using UnityEngine;
using UnityEngine.Events;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager profile;
    [SerializeField] private int _money;
    [SerializeField] private float _weaponMaterials;
    [SerializeField] private int _denaries;
    [SerializeField] private float _lvl;
    [SerializeField] private float _exp;
    [SerializeField] private int _character = 0; //0 is default hero

    public UnityAction valuesChanged;

    private void OnEnable()
    {
        if (!profile)
        {
            profile = this;
        }
        else if (profile == this)
        {
            Destroy(this);
        }

        #region Check Keys
        if (!PlayerPrefs.HasKey("charID"))
        {
            PlayerPrefs.SetInt("charID", _character);
        }
        else
        {
            _character = PlayerPrefs.GetInt("charID", 0);
        }

        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetInt("money", _money);
        }
        else
        {
            _money = PlayerPrefs.GetInt("money");
        }

        if (!PlayerPrefs.HasKey("denary"))
        {
            PlayerPrefs.SetInt("denary", _denaries);
        }
        else
        {
            _denaries = PlayerPrefs.GetInt("denary");
        }

        if (!PlayerPrefs.HasKey("weaponMaterials"))
        {
            PlayerPrefs.SetFloat("weaponMaterials", _weaponMaterials);
        }
        else
        {
            _weaponMaterials = PlayerPrefs.GetFloat("weaponMaterials");
        }


        if (!PlayerPrefs.HasKey("lvl"))
        {
            PlayerPrefs.SetFloat("lvl", _lvl);
        }
        else
        {
            _lvl = PlayerPrefs.GetFloat("lvl", _lvl);
        }


        if (!PlayerPrefs.HasKey("exp"))
        {
            PlayerPrefs.SetFloat("exp", _exp);
        }
        else
        {
            _exp = PlayerPrefs.GetFloat("exp");
        }


        if (!PlayerPrefs.HasKey("inventoryMain"))
        {
            PlayerPrefs.SetString("inventoryMain", "");
        }
        #endregion
    }

    public void OnValuesChanged()
    {
        valuesChanged();
    }


    public string GetSaveKeyName(Keys keyName)
    {
        switch (keyName)
        {
            case Keys.money:
                return "money";

            case Keys.denary:
                return "denary";

            case Keys.weaponMats:
                return "weaponMaterials";

            case Keys.exp:
                return "exp";

            case Keys.lvl:
                return "lvl";

            case Keys.inventoryString:
                return "inventoryMain";

            default:
                return "";
        }
    }

}

public enum Keys
{
    money, denary, weaponMats, exp, lvl, inventoryString
}
