using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonHolder : MonoBehaviour
{

    [SerializeField] private GameObject _attentionCircle;
    [SerializeField] private GameObject _skillTab;

   [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(OpenSkillTab);
    }

    void Start()
    {
        _attentionCircle.SetActive(false);
        if (PlayerPrefs.GetInt("point") >= 1)
        {
            _attentionCircle.SetActive(true);
        }
    }

    void OpenSkillTab()
    {
        _attentionCircle.SetActive(false);

    }

}
