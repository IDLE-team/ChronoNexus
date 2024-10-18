using DG.Tweening;
using System.Collections;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _slider;

    [SerializeField] private bool isOnGame = false;
    [SerializeField] private GameObject _iconSkillPoint;

    [Header("Для значений из таблицы баланса уровней")]
    [SerializeField] private float _xpMeanLvl = 50;
    [SerializeField] private float _xpStepMean = 15;
    [SerializeField] private float _xpToNextBase = 150;

    private int _level;

    //значения должны совпадать с WinScreen

    private void Start()
    {
        ExpChange();
        if (!isOnGame)
        {
            _iconSkillPoint.SetActive(false);
            if (PlayerPrefs.GetInt("point", 0) > 0)
            {
                _iconSkillPoint.SetActive(true);
            }
        }
    }

    public void ExpChange()
    {
        _level = PlayerPrefs.GetInt("lvl", 1);
        StartCoroutine(LevelUp(0, _level, 0, PlayerPrefs.GetInt("exp", 0)));
        _levelText.text = _level.ToString();
    }

    public float AddExp(float minAmountShare, float maxAmountShare)
    {
        float expAdd = _slider.maxValue * Random.Range(minAmountShare, maxAmountShare);

        if (expAdd + _slider.value >= _slider.maxValue)
        {
            StartCoroutine(LevelUp(0, _level, (int)_slider.value, (int)expAdd));
        }
        else
        {
            _slider.DOValue((int)_slider.value + expAdd, 1f).SetDelay(1f);
        }

        return expAdd;
    }

    private IEnumerator LevelUp(int delay, int lvl, int startExp, int exp)
    {

        _levelText.text = lvl.ToString();
        int xpToNextLevel = GetExpToLevel(lvl+1);
        _slider.maxValue = xpToNextLevel;

        _slider.value = startExp;

        if (exp + startExp >= xpToNextLevel)
        {
            _slider.DOValue(xpToNextLevel, 1f).SetDelay(1f);
            startExp = 0;
            exp -= xpToNextLevel;
            lvl++;

            PlayerPrefs.SetInt("point", PlayerPrefs.GetInt("point", 0) + 1);
            PlayerPrefs.SetInt("lvl", lvl);
        }
        else
        {
            _slider.DOValue(exp, 1f).SetDelay(1f);
            yield return null;
            yield break;
        }
        yield return new WaitForSeconds(0);
        StartCoroutine(LevelUp(0, lvl, startExp, exp));

    }



    private int GetExpToNextLevel()
    {
        int lvl = PlayerPrefs.GetInt("lvl", 0);
        return (int)(Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl);
    }
    private int GetExpToLevel(int lvl)
    {
        return (int)(Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl);
    }
}
