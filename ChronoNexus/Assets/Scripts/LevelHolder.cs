using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _slider;

    [Header("Для значений из таблицы баланса уровней")]
    [SerializeField] private float _xpMeanLvl = 50;
    [SerializeField] private float _xpStepMean = 15;
    [SerializeField] private float _xpToNextBase = 150;

    //значения должны совпадать с WinScreen

    private void Start()
    {
        XpChange();
    }

    public void XpChange()
    {
        var lvl = PlayerPrefs.GetFloat("lvl",1);
        _slider.maxValue = Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl;
        _slider.DOValue(PlayerPrefs.GetFloat("xp",0), 1f);
        _levelText.text = lvl.ToString();
    }
}
