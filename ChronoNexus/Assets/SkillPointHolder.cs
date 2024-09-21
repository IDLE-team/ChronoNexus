using TMPro;
using UnityEngine;

public class SkillPointHolder : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _skillPointText;

    private int _skillPointValue;
    private string _skillText;

    private void Start()
    {
        _skillPointValue = PlayerPrefs.GetInt("point", 0);
        _skillPointText = GetComponent<TextMeshProUGUI>();
        _skillText = _skillPointText.text;

        PlayerProfileManager.profile.pointChanged += OnPointChanged;
        OnPointChanged();
    }

    private void OnPointChanged()
    {
        _skillPointText.text = _skillText + _skillPointValue.ToString();
        SavePoint();
    }

    private void SavePoint()
    {
        PlayerPrefs.SetInt("point", _skillPointValue);
    }

    public void DecreasePoint()
    {
        _skillPointValue--;
        OnPointChanged();
    }

    private void OnDisable()
    {
        PlayerProfileManager.profile.pointChanged -= OnPointChanged;
        SavePoint();
    }

}
