using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyGainText;
    [SerializeField] private TextMeshProUGUI _xpGainText;
    [SerializeField] private TextMeshProUGUI _levelCurrentText;
    [SerializeField] private TextMeshProUGUI _levelNextText;
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Slider _levelSliderShadow;

    [SerializeField] private Button _buttonLoadMenu;

    [Header("Для значений из таблицы баланса уровней")]
    [SerializeField] private float _xpMeanLvl = 50;
    [SerializeField] private float _xpStepMean = 15;
    [SerializeField] private float _xpToNextBase = 150;


    private SceneLoader _sceneLoader;

    private float xpToNextLvl, startXp;
    private float lvl;

    private void Start()
    {
        _buttonLoadMenu.onClick.AddListener(LoadScene);
        transform.localScale = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            print(GetExpToNextLevel(i));
        }
    }

    public void SetScreen(LevelStatTracker tracker, SceneLoader loader)
    {
        startXp = PlayerPrefs.GetFloat("xp", 0);
        lvl = PlayerPrefs.GetFloat("lvl", 1);

        xpToNextLvl = GetExpToNextLevel(lvl);

        

        transform.DOScale(1, 0.4f);

        float earnedMoney = 0, earnedXp = 0;

        earnedMoney = tracker.GetKilledEnemyAmount() * Random.Range(20, 30) ;
        _moneyGainText.text = earnedMoney.ToString();

        earnedXp = tracker.GetKilledEnemyAmount() * Random.Range(30, 50) / 10;


        _xpGainText.text = earnedXp.ToString();

        _sceneLoader = loader;


        _levelCurrentText.text = lvl.ToString();
        _levelNextText.text = (lvl + 1).ToString();

        
        print(xpToNextLvl);
        _levelSlider.maxValue = xpToNextLvl;
        _levelSliderShadow.maxValue = xpToNextLvl;

        if (earnedXp + startXp >= xpToNextLvl)
        {
            earnedXp += startXp - xpToNextLvl;
            _levelSliderShadow.DOValue(_levelSliderShadow.maxValue, 1f);
            _levelSlider.DOValue(_levelSlider.maxValue, 1f).SetDelay(1f);
            StartCoroutine(LevelUp(2f, earnedXp, earnedMoney));
        }
        else
        {
            _levelSlider.value = startXp;
            _levelSliderShadow.value = startXp;
            _levelSliderShadow.DOValue(startXp + earnedXp, 1f);
            _levelSlider.DOValue(startXp + earnedXp, 1f).SetDelay(1f);
            _xpText.text = (earnedXp + startXp).ToString() + "/" + xpToNextLvl.ToString();

            PlayerPrefs.SetFloat("xp", PlayerPrefs.GetFloat("xp", 0) + earnedXp);
            PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) + earnedMoney);
        }

    }

    private void LoadScene()
    {
        _sceneLoader.SceneToLoad();
    }

    private IEnumerator LevelUp(float delay, float xp, float earnedMoney)
    {
        yield return new WaitForSeconds(delay);
        lvl++;
        var xpToNextLevel = GetExpToNextLevel(lvl);
        PlayerPrefs.SetFloat("lvl", lvl);
        _levelCurrentText.text = lvl.ToString();
        _levelNextText.text = (lvl + 1).ToString();

        _levelSliderShadow.maxValue = xpToNextLevel;
        _levelSlider.maxValue = xpToNextLevel;

        _levelSlider.value = 0;
        _levelSliderShadow.value = 0;

        _levelSliderShadow.DOValue(xp , 1f);
        _levelSlider.DOValue(xp, 1f).SetDelay(1f);

        _xpText.text = (xp).ToString() + "/" + xpToNextLevel.ToString();

        PlayerPrefs.SetFloat("xp",xp);
        PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) + earnedMoney);

        if (xp >= xpToNextLvl)
        {
            xp -= xpToNextLvl;
            _levelSliderShadow.DOValue(_levelSliderShadow.maxValue, 1f);
            _levelSlider.DOValue(_levelSlider.maxValue, 1f).SetDelay(1f);
            StartCoroutine(LevelUp(2f, xp, earnedMoney));
        }
    }

        private float GetExpToNextLevel(float lvl)
    {
        return Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl;
    }
}
