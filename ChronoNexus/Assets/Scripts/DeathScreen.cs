using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Button _buttonBackToMenu;
    [SerializeField] private Button _buttonTryAgain;

    private void Start()
    {
        _buttonBackToMenu.onClick.AddListener(BackToMenu);
        _buttonTryAgain.onClick.AddListener(RestartLevel);
    }
    private void OnEnable()
    {
        transform.DOScale(1, 0.6f);
    }
    private void OnDisable()
    {
        transform.DOScale(0, 0.6f);
    }
    public void RestartLevel()
    {
        LevelController.instance.Restart();
    }
    public void BackToMenu()
    {
        LevelController.instance.LoadSceneWithTransition("UI_Scene");
    }

}
