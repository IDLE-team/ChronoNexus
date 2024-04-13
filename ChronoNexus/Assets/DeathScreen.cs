using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private Button _buttonBackToMenu;
    [SerializeField] private Button _buttonTryAgain;

    [SerializeField] private Health _heroHealth;

    private void Start()
    {
        _buttonBackToMenu.onClick.AddListener(BackToMenu);
        _buttonTryAgain.onClick.AddListener(RestartLevel);
        _heroHealth.Died += Death;
        // _HeroSlider.onValueChanged.AddListener(CheckDeath);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Death()
    {
        transform.DOScale(1, 0.6f);
    }

    public void BackToMenu()
    {
        _sceneLoader.SceneToLoad();
    }

    private void CheckDeath(float value)
    {
        if (value <= 0)
        {
            Death();
        }
    }
}
