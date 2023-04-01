using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InputProvider inputProvider;

    public InputProvider InputProvider => inputProvider;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}