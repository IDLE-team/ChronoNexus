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

    private void Start()
    {
        _buttonLoadMenu.onClick.AddListener(LoadScene);
        transform.localScale = Vector3.zero;

    }

    public void SetScreen(LevelStatTracker tracker, SceneLoader loader)
    {
        var startXp = PlayerPrefs.GetFloat("xp", 0);
        transform.DOScale(1, 0.4f);

        float money = 0, xp = 0;

        for (int i = 0; i < tracker.GetKilledEnemyAmount(); i++)
        {
            money += Random.Range(20, 40);
        }


        _moneyGainText.text = money.ToString();


        for (int i = 0; i < tracker.GetKilledEnemyAmount(); i++)
        {
            xp += Random.Range(2, 8);
        }


        _xpGainText.text = xp.ToString();

        _sceneLoader = loader;

        var lvl = PlayerPrefs.GetFloat("lvl", 1);
        _levelCurrentText.text = lvl.ToString();
        _levelNextText.text = (lvl + 1).ToString();

        var xpToNextLvl = GetExpToNextLevel(lvl);
        print(xpToNextLvl);
        _levelSlider.maxValue = xpToNextLvl;
        _levelSliderShadow.maxValue = xpToNextLvl;

        if (xp + startXp >= xpToNextLvl)
        {
            _levelSliderShadow.DOValue(startXp + xp, 1f);
            _levelSlider.DOValue(startXp + xp, 1f).SetDelay(1f);
            StartCoroutine(LevelUp(2f, lvl, xp, startXp, money, xp + startXp - xpToNextLvl));
        }
        else
        {
            _levelSlider.value = startXp;
            _levelSliderShadow.value = startXp;
            _levelSliderShadow.DOValue(startXp + xp, 1f);
            _levelSlider.DOValue(startXp + xp, 1f).SetDelay(1f);
            _xpText.text = (xp + startXp).ToString() + "/" + xpToNextLvl.ToString();

            PlayerPrefs.SetFloat("xp", PlayerPrefs.GetFloat("xp", 0) + xp+startXp);
            PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) + money);
        }

    }

    private void LoadScene()
    {
        _sceneLoader.SceneToLoad();
    }

    private IEnumerator LevelUp(float delay, float lvl, float xp, float startXp, float money, float xpPrev)
    {
        yield return new WaitForSeconds(delay);
        lvl++;
        var xpToNextLevel = GetExpToNextLevel(lvl);
        PlayerPrefs.SetFloat("lvl", lvl);
        _levelCurrentText.text = lvl.ToString();
        _levelNextText.text = (lvl + 1).ToString();

        _levelSliderShadow.maxValue = GetExpToNextLevel(lvl);
        _levelSlider.maxValue = GetExpToNextLevel(lvl);

        _levelSlider.value = 0;
        _levelSliderShadow.value = 0;

        _levelSliderShadow.DOValue(xp + startXp - xpToNextLevel, 1f);
        _levelSlider.DOValue(xp + startXp - xpToNextLevel, 1f).SetDelay(1f);

        _xpText.text = (xpPrev).ToString() + "/" + xpToNextLevel.ToString();

        PlayerPrefs.SetFloat("xp", PlayerPrefs.GetFloat("xp", 0) + xpPrev);
        PlayerPrefs.SetFloat("money", PlayerPrefs.GetFloat("money", 0) + money);
    }

    private float GetExpToNextLevel(float lvl)
    {
        return Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl;
    }
}
