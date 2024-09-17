using TMPro;
using UnityEngine;

public class MaterialHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _materialText;

    private float _materialValue;

    private void Start()
    {
        _materialValue = PlayerPrefs.GetFloat("material", 0);
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
        PlayerPrefs.SetFloat("material", _materialValue);
    }

    public float GetMaterialValue()
    {
        return _materialValue;
    }

    public void DecreaseMoneyValue(float cost)
    {
        _materialValue -= cost;
        PlayerProfileManager.profile.moneyChanged();
    }


    private void OnDestroy()
    {
        PlayerProfileManager.profile.materialChanged -= OnMaterialChange;
        PlayerProfileManager.profile.materialChanged -= SaveMaterial;
        SaveMaterial();
    }
}
