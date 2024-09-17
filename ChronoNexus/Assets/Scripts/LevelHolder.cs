using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _iconSkillPoint;

    [Header("��� �������� �� ������� ������� �������")]
    [SerializeField] private float _xpMeanLvl = 50;
    [SerializeField] private float _xpStepMean = 15;
    [SerializeField] private float _xpToNextBase = 150;

    //�������� ������ ��������� � WinScreen

    private void Start()
    {
        ExpChange();
    }

    public void ExpChange()
    {
        var lvl = PlayerPrefs.GetInt("lvl", 1) + 1;
        _slider.maxValue = Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl;
        _slider.DOValue(PlayerPrefs.GetInt("exp", 0), 1f);
        _levelText.text = (lvl - 1).ToString();
    }
}
