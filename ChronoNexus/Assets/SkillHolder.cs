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

    private SkillScriptableObject _skill;
    private Color _skillColorScheme;

    public void InitializeSkillVisual(SkillScriptableObject skill)
    {
        _skill = skill;

        _skillColorScheme = _skill.ReturnColorByType(_skill.type);

        _skillIconBack.color = _skillColorScheme;

        _textSkillDescription.text = _skill.skillDescription; // + _skill.levelDescription[_skill.currentLvl] ;
        _textSkillName.text = _skill.skillName;
        _textSkillLvl.text = _skill.currentLvl.ToString() + "/" + _skill.maxLvl.ToString();
        _skillIcon.sprite = _skill.skillIconImage;
        _skillIcon.color = Color.white;
    }
}
