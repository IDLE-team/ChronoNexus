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
        _winScreen.SetScreen(rewards, PlayerPrefs.GetInt("exp", 1), PlayerPrefs.GetInt("lvl", 1));
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
        rewards.Material = _levelStatTracker.GetMaterialCount(); 
        return rewards;

    }

    public void ApplyReward(Rewards rewards)
    {
        int currentExp = PlayerPrefs.GetInt("exp", 0);
        int currentLvl = PlayerPrefs.GetInt("lvl", 0);
        int currentMoney = PlayerPrefs.GetInt("money", 0);
        int currentMaterials = PlayerPrefs.GetInt("material", 0);
        int newExp = 0;

        PlayerPrefs.SetInt("money", currentMoney + rewards.Money);
        newExp = currentExp + rewards.Experience;
        while (newExp >= GetExpToNextLevel())
        {
            newExp = (int)Mathf.Abs(newExp - GetExpToNextLevel());
            PlayerPrefs.SetInt("lvl", PlayerPrefs.GetInt("lvl", 0) + 1);
        }
        PlayerPrefs.SetInt("exp", newExp);
        PlayerPrefs.SetInt("material", currentMaterials + rewards.Material);
    }

    public int GetExpToNextLevel()
    {
        int lvl = PlayerPrefs.GetInt("lvl", 0);
        return (int)(Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl);
    }
    public int GetExpToLevel(int lvl)
    {
        return (int)(Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) - Mathf.Round((_xpMeanLvl + lvl * _xpStepMean) * 2 + _xpToNextBase * 1.1f) % _xpMeanLvl);
    }

    public void AddMaterials(int count)
    {
        _levelStatTracker.AddMaterials(count);
    }
}
