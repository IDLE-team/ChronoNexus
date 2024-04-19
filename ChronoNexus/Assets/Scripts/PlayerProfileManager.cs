using UnityEngine;
using UnityEngine.Events;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager profile;
    [SerializeField] private float _money;
    [SerializeField] private float _lvl;
    [SerializeField] private float _exp;

    public UnityAction moneyChanged;
    public UnityAction expChanged;

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

        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetFloat("money", _money);
        }
        else
        {
            PlayerPrefs.GetFloat("money");
        }
        if (!PlayerPrefs.HasKey("lvl"))
        {
            PlayerPrefs.SetFloat("lvl", _lvl);
        }
        else
        {
            PlayerPrefs.GetFloat("lvl", _lvl);
        }
        if (!PlayerPrefs.HasKey("exp"))
        {
            PlayerPrefs.SetFloat("exp", _exp);
        }
        else
        {
            PlayerPrefs.GetFloat("exp");
        }

        if (!PlayerPrefs.HasKey("inventoryMain"))
        {
            PlayerPrefs.SetString("inventoryMain", "");
        }
    }

    public void OnMoneyChange()
    {
        moneyChanged();
    }
    public void OnExpChange()
    {
        expChanged();
    }
}
