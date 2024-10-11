using TMPro;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _materialText;

    private int _materialValue;

    private void Start()
    {
        _materialValue = PlayerPrefs.GetInt("material", 0);
        _materialText = GetComponent<TextMeshProUGUI>();
        PlayerProfileManager.profile.materialChanged += OnMaterialChange;
        PlayerProfileManager.profile.materialChanged += SaveMaterial;
        OnMaterialChange();
    }

    public void OnMaterialChange()
    {
        _materialText.text = _materialValue.ToString();
    }

    private void SaveMaterial()
    {
        PlayerPrefs.SetInt("material", _materialValue);
    }

    public float GetMaterialValue()
    {
        return _materialValue;
    }

    public bool DecreaseMaterialValue(int cost)
    {
        if (cost >= 0 && _materialValue - cost >=0)
        {
            _materialValue -= cost;
            PlayerProfileManager.profile.materialChanged();
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void IncreaseMaterialValue(int cost)
    {
        if (cost > 0)
        {
            _materialValue+= cost;
            PlayerProfileManager.profile.materialChanged();
        }
    }


    private void OnDestroy()
    {
        PlayerProfileManager.profile.materialChanged -= OnMaterialChange;
        PlayerProfileManager.profile.materialChanged -= SaveMaterial;
        SaveMaterial();
    }
}
