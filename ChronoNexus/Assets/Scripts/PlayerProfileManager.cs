using UnityEngine;
using UnityEngine.Events;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager profile;
    [SerializeField] private float _money;
    [SerializeField] private float _materials;
    [SerializeField] private float _lvl;
    [SerializeField] private float _exp;
    [SerializeField] private float _point;

    public float Point => _point;
    
    public UnityAction moneyChanged;
    public UnityAction expChanged;
    public UnityAction lvlChanged;
    public UnityAction materialChanged;

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

        if (!PlayerPrefs.HasKey("material"))
        {
            PlayerPrefs.SetFloat("material", _materials);
        }
        else
        {
            PlayerPrefs.GetFloat("material");
        }

        if (!PlayerPrefs.HasKey("lvl"))
        {
            PlayerPrefs.SetFloat("lvl", _lvl);
        }
        else
        {
            PlayerPrefs.GetFloat("lvl", _lvl);
        }

        if (!PlayerPrefs.HasKey("point"))
        {
            PlayerPrefs.SetFloat("point", _point);
        }
        else
        {
            PlayerPrefs.GetFloat("point", _point);
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

    public void OnLvlChange()
    {
        lvlChanged();
    }
}
