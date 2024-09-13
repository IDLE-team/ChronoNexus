using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private TextMeshProUGUI _moneyGainText;
    [SerializeField] private TextMeshProUGUI _xpGainText;
    [SerializeField] private TextMeshProUGUI _materialText;
    [SerializeField] private TextMeshProUGUI _levelCurrentText;
    [SerializeField] private TextMeshProUGUI _levelNextText;
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Slider _levelSliderShadow;

    [SerializeField] private Button _buttonLoadMenu;

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.4f);
    }
    private void OnDisable()
    {
        transform.DOScale(0, 0.4f);
    }
    private void Start()
    {
        _buttonLoadMenu.onClick.AddListener(LoadScene);
    }

    public void SetScreen(Rewards rewards, float startExp, float lvl)
    {
        if (rewards.Money == 0)
        {
            _moneyGainText.GetComponentInParent<Image>().gameObject.SetActive(false);
        }
        else
        {
            _moneyGainText.text = rewards.Money.ToString();
        }

        if (rewards.Experience == 0)
        {
            _xpGainText.GetComponentInParent<Image>().gameObject.SetActive(false);
        }
        else
        {
            _xpGainText.text = rewards.Experience.ToString();
        }

        if (rewards.Material == 0)
        {
            _materialText.GetComponentInParent<Image>().gameObject.SetActive(false);
        }
        else
        {
            _materialText.text = rewards.Material.ToString();
        }

        _levelCurrentText.text = lvl.ToString();
        _levelNextText.text = (lvl + 1).ToString();

        var xpToNextLvl = GameController.Instance.GetExpToNextLevel();
        _levelSlider.maxValue = xpToNextLvl;
        _levelSliderShadow.maxValue = xpToNextLvl;

        if (rewards.Experience + startExp >= xpToNextLvl)
        {
            // _levelSliderShadow.DOValue(startExp + rewards.Experience, 1f);
            //  _levelSlider.DOValue(startExp + rewards.Experience, 1f).SetDelay(1f);
            // _levelSlider.value = startExp;
            //  _levelSliderShadow.value = startExp;

            // _levelSliderShadow.DOValue(xpToNextLvl, 1f);
            // _levelSlider.DOValue(xpToNextLvl, 1f).SetDelay(1f);
            //  StartCoroutine(LevelUp(2f, lvl,
            //   Mathf.Abs(rewards.Experience + startExp - xpToNextLvl)));
            StartCoroutine(LevelUp(2f, lvl, startExp, rewards.Experience + startExp));
        }
        else
        {
            _levelSlider.value = startExp;
            _levelSliderShadow.value = startExp;
            _levelSliderShadow.DOValue(startExp + rewards.Experience, 1f);
            _levelSlider.DOValue(startExp + rewards.Experience, 1f).SetDelay(1f);
            _xpText.text = (rewards.Experience + startExp).ToString() + "/" + xpToNextLvl.ToString();
        }
    }

    private void LoadScene()
    {
        LevelController.instance.LoadSceneWithTransition(_sceneToLoad);
    }

    private IEnumerator LevelUp(float delay, float lvl, float startExp, float exp)
    {

        Debug.Log("LevelUP сработал");
        var xpToNextLevel = GameController.Instance.GetExpToLevel(lvl);
        _xpText.text = exp + "/" + xpToNextLevel;
        _levelCurrentText.text = lvl.ToString();
        _levelNextText.text = (lvl + 1).ToString();

        _levelSliderShadow.maxValue = xpToNextLevel;
        _levelSlider.maxValue = xpToNextLevel;

        _levelSlider.value = startExp;
        _levelSliderShadow.value = startExp;

        if (exp >= xpToNextLevel)
        {
            _levelSliderShadow.DOValue(xpToNextLevel, 1f);
            _levelSlider.DOValue(xpToNextLevel, 1f).SetDelay(1f);
            startExp = 0;
            exp -= xpToNextLevel;
            lvl++;
            PlayerPrefs.SetInt("point", PlayerPrefs.GetInt("point", 0) + 1);
        }
        else
        {
            _levelSliderShadow.DOValue(exp, 1f);
            _levelSlider.DOValue(exp, 1f).SetDelay(1f);
            yield return null;
            yield break;
        }
        yield return new WaitForSeconds(delay);
        StartCoroutine(LevelUp(2, lvl, startExp, exp));

    }
}