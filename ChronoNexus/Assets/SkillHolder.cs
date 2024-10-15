using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textSkillName;
    [SerializeField] private TextMeshProUGUI _textSkillDescription;
    [SerializeField] private TextMeshProUGUI _textSkillLvl;
    [SerializeField] private TextMeshProUGUI _textProgressValuePrefab;
    [SerializeField] private TextMeshProUGUI _separatorPrefab;
    [SerializeField] private Transform _skillProgressHolder;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _skillIconBack;
    [SerializeField] private Button _button;

    private List<TextMeshProUGUI> _progressValues = new List<TextMeshProUGUI>();
    private TextMeshProUGUI _previousProgressValue;
    private SkillScriptableObject _skill;
    private Color _skillColorScheme;
    private Color _inactiveColor;
    private SkillPointHolder _skillPointHolder;

    public void InitializeSkill(SkillScriptableObject skill)
    {
        _skillPointHolder = GetComponentInParent<UserInterfaceComponent>().GetComponentInChildren<SkillPointHolder>();

        _skill = skill;

        _skillColorScheme = _skill.ReturnColorByType(_skill.type);

        _skillIconBack.color = _skillColorScheme;

        _textSkillDescription.text = _skill.skillDescription; // + _skill.levelDescription[_skill.currentLvl] ;
        _textSkillName.text = _skill.skillName;
        _textSkillLvl.text = _skill.currentLvl + "/" + _skill.maxLvl.ToString();
        _skillIcon.sprite = _skill.skillIconImage;
        _skillIcon.color = Color.white;
        _inactiveColor = _textProgressValuePrefab.color;
        for (int i = 0; i < skill.upgradeValuePerLevel.Count; i++)
        {
            if (i > 0)
            {
                Instantiate(_separatorPrefab, _skillProgressHolder);
            }

            TextMeshProUGUI progressValue = Instantiate(_textProgressValuePrefab, _skillProgressHolder);
            progressValue.text = skill.progressValueText.Replace("[value]", skill.upgradeValuePerLevel[i].ToString());
            _progressValues.Add(progressValue);
        }

        if (_skill.currentLvl > 0)
        {
            UpdateSelectedProgressValue();
        }

        _button.onClick.AddListener(TryUpgrade);
    }

    public void TryUpgrade()
    {
        if (_skill.currentLvl >= _skill.maxLvl)
            return;
        if (PlayerPrefs.GetInt("point", 0) == 0)
            return;

        _skillPointHolder.DecreasePoint();

        _skill.currentLvl++;
        _textSkillLvl.text = _skill.currentLvl + "/" + _skill.maxLvl;
        UpdateSelectedProgressValue();

        UpgradeData.Instance.SetStat(_skill.upgradeType, _skill.GetUpgradeValue());
    }

    private void UpdateSelectedProgressValue()
    {
        if (_previousProgressValue != null)
        {
            _previousProgressValue.color = _inactiveColor;
            _previousProgressValue.fontStyle = FontStyles.Normal;
        }

        _progressValues[_skill.currentLvl - 1].color = _skill._progressValueTextColor;
        _progressValues[_skill.currentLvl - 1].fontStyle = FontStyles.Bold;
        _previousProgressValue = _progressValues[_skill.currentLvl - 1];
    }
}