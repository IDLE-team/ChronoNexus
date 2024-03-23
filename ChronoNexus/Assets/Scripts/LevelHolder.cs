using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _slider;

    private void Start()
    {
        _slider.maxValue = PlayerPrefs.GetFloat("lvl") * 100;
        _slider.value = PlayerPrefs.GetFloat("exp");
        _levelText.text = PlayerPrefs.GetFloat("lvl").ToString();

        PlayerProfileManager.profile.expChanged += OnExpChanged;
    }

    private void OnExpChanged()
    {
        var exp = PlayerPrefs.GetFloat("exp");
        var lvl = PlayerPrefs.GetFloat("lvl");
        _slider.DOValue(exp, 0.7f);
        if (exp > lvl * 100)
        {
            PlayerPrefs.SetFloat("lvl", lvl + 1);
            PlayerPrefs.SetFloat("exp", exp - (lvl + 1)*100);
        }
    }
}
