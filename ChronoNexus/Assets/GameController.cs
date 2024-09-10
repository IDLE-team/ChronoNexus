using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{   
    static public GameController Instance;

    [Inject]
    private Character _character;
    [Inject]
    private WinScreen _winScreen;
    [Inject]
    private DeathScreen _deathScreen;
    
    [SerializeField] private LevelStatTracker _levelStatTracker; 
    
    [Header("Для значений из таблицы баланса уровней")]
    [SerializeField] private float _xpMeanLvl = 50;
    [SerializeField] private float _xpStepMean = 15;
    [SerializeField] private float _xpToNextBase = 150;
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void Win()
    {
        Rewards rewards = CalculateLevelReward();
        _winScreen.gameObject.SetActive(true);
        _winScreen.SetScreen(rewards, PlayerPrefs.GetFloat("xp", 1), PlayerPrefs.GetFloat("lvl", 1));
        _character.SetInvincible(true);
        ApplyReward(rewards);
    }

    public void Death()
    {
        _deathScreen.gameObject.SetActive(true);
    }
    public Rewards CalculateLevelReward()
    {
        Rewards rewards = new Rewards();
        int money = 0;
        int xp = 0;
        
        for (int i = 0; i < _levelStatTracker.GetKilledEnemyAmount(); i++)
        {
            money += Random.Range(20, 40);
            xp += Random.Range(5, 20);
        }
        Debug.Log("Killed: " + _levelStatTracker.GetKilledEnemyAmount());
        Debug.Log("ExpForKills: " + xp);
        rewards.Money = money;
        rewards.Experience = xp;
        return rewards;
    }

    public void ApplyReward(Rewards rewards)
    {
        float currentExp = PlayerPrefs.GetFloat("xp", 0);
        float currentLvl = PlayerPrefs.GetFloat("lvl", 0);
        float currentMoney = PlayerPrefs.GetFloat("money", 0);
        float newExp = 0;

        PlayerPrefs.SetFloat("money", currentMoney + rewards.Money);
        newExp = currentExp + rewards.Experience;
        while (newExp >= GetExpToNextLevel())
        {
            newExp = Mathf.Abs( newExp - GetExpToNextLevel());
            PlayerPrefs.SetFloat("lvl", PlayerPrefs.GetFloat("lvl",0) + 1 );
        }
        PlayerPrefs.SetFloat("xp", newExp);
        
    }

    public float GetExpToNextLevel()
    {
        float lvl = PlayerPrefs.GetFloat("lvl", 0);
        return Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl;
    }
    public float GetExpToLevel(float lvl)
    {
        return Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl;
    }
}
