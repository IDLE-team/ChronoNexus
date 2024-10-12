using UnityEngine;
using UnityEngine.Events;

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager profile;
    [SerializeField] private int _money;
    [SerializeField] private int _materials;
    [SerializeField] private int _lvl;
    [SerializeField] private int _exp;
    [SerializeField] private int _point;
    [SerializeField] private int _hero = 0;

    public float Point => _point;
    
    public UnityAction moneyChanged;
    public UnityAction pointChanged;
    public UnityAction expChanged;
    public UnityAction lvlChanged;
    public UnityAction materialChanged;
    public UnityAction itemChanged;
    public UnityAction heroChanged;

    private void Awake()
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
            PlayerPrefs.SetInt("money", _money);
        }
        else
        {
            PlayerPrefs.GetInt("money");
        }

        if (!PlayerPrefs.HasKey("material"))
        {
            PlayerPrefs.SetInt("material", _materials);
        }
        else
        {
            PlayerPrefs.GetInt("material");
        }

        if (!PlayerPrefs.HasKey("lvl"))
        {
            PlayerPrefs.SetInt("lvl", _lvl);
        }
        else
        {
            PlayerPrefs.GetInt("lvl", _lvl);
        }

        if (!PlayerPrefs.HasKey("point"))
        {
            PlayerPrefs.SetInt("point", _point);
        }
        else
        {
            PlayerPrefs.GetInt("point", _point);
        }

        if (!PlayerPrefs.HasKey("hero"))
        {
            PlayerPrefs.SetInt("hero", _hero);
        }
        else
        {
            PlayerPrefs.GetInt("hero", _hero);
        }



        if (!PlayerPrefs.HasKey("exp"))
        {
            PlayerPrefs.SetInt("exp", _exp);
        }
        else
        {
            PlayerPrefs.GetInt("exp");
        }

        if (!PlayerPrefs.HasKey("inventoryMain"))
        {
            PlayerPrefs.SetString("inventoryMain", "");
        }

        itemChanged += OnItemChanged;
    }


    public void OnMoneyChange()
    {
        moneyChanged();
    }
    public void OnPointChanged()
    {
        pointChanged();
    }
    public void OnExpChange()
    {
        expChanged();
    }
    public void OnLvlChange()
    {
        lvlChanged();
    }

    public void OnMaterialChanged()
    {
        materialChanged();
    }

    public void OnItemChanged()
    {
        // should be empty DO NOT TOUCH
    }

    public void OnHeroChanged()
    {
        heroChanged();
    }
}
