using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textSkillName;
    [SerializeField] private TextMeshProUGUI _textSkillDescription;
    [SerializeField] private TextMeshProUGUI _textSkillLvl;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _skillIconBack;

    [SerializeField] private Button _button;

    private SkillScriptableObject _skill;
    private Color _skillColorScheme;

    public void InitializeSkill(SkillScriptableObject skill)
    {
        _skill = skill;

        _skillColorScheme = _skill.ReturnColorByType(_skill.type);

        _skillIconBack.color = _skillColorScheme;

        _textSkillDescription.text = _skill.skillDescription; // + _skill.levelDescription[_skill.currentLvl] ;
        _textSkillName.text = _skill.skillName;
        _textSkillLvl.text = _skill.currentLvl + "/" + _skill.maxLvl.ToString();
        _skillIcon.sprite = _skill.skillIconImage;
        _skillIcon.color = Color.white;
        _button.onClick.AddListener(TryUpgrade);
    }

    public void TryUpgrade()
    {
        if (_skill.currentLvl >= _skill.maxLvl)
            return;
        if(PlayerProfileManager.profile.Point < 1)
            return;
        _skill.currentLvl++;
        _textSkillLvl.text = _skill.currentLvl+ "/" + _skill.maxLvl;

        UpgradeData.Instance.SetStat(_skill.upgradeType, _skill.GetUpgradeValue());
        
    }
}