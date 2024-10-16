using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

public class DailyQuestSystem : MonoBehaviour
{

    [Header("Текущие квесты")]
    [Header("---------------------------------")]

    [Tooltip("Текущие квесты, автоматическая подстановка, не трогать")]
    [SerializeField] private List<QuestData> _currentQuests = new List<QuestData>();

    [Header("Настройка")]
    [Header("---------------------------------")]
    [SerializeField] private int _maxQuestsPerDay;
    [SerializeField] private  List<QuestData> quests = new List<QuestData>(); 
    
    [Header("Ссылки")]
    [Header("---------------------------------")]
    [SerializeField] private QuestUISetter _questUISetter;
    [SerializeField] private TextMeshProUGUI questText;
    
    public List<QuestData> CurrentQuests => _currentQuests;
    public static DailyQuestSystem Instance;
    public Action OnGenerated;
    private bool _sceneLoaded = false;
    private int progress;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        transform.SetParent(null);
        if (Instance != null)
            Debug.Log(Instance._currentQuests.Count);
        //Instance._questUISetter = _questUISetter; 
        //SetCurrentQuestsUI();

    }

    [Inject]
    public void Construct(QuestUISetter questUISetter)
    {
        _questUISetter = questUISetter;
    }

    public void SetCurrentQuestsUI()
    {
        if (Instance._currentQuests.Count <= 0)
        {
            return;
        }

        for (int i = 0; i <Instance._currentQuests.Count; i++)
        {
            Instance.OnGenerated?.Invoke();
          //  Instance._questUISetter.SetQuestUI(_currentQuests[i]);
        }
    }
    void Start()
    {
        GenerateNewQuest();
    }

    void GenerateNewQuest()
    {
        for (int i = 0; i < _maxQuestsPerDay; i++)
        {
            if (i > quests.Count-1)
            {
                break;
            }

            var newQuest = quests[Random.Range(0, quests.Count)];
            
            if (_currentQuests.Count<= 0)
                _currentQuests.Add(newQuest);
            else
            {
                bool isUnique = false;
                
                while (!isUnique)
                {
                    newQuest = quests[Random.Range(0, quests.Count)];
                    isUnique = true;

                    foreach (var quest in _currentQuests)
                    {
                        if (quest == newQuest)
                        {
                            isUnique = false;
                            break;
                        }
                    }
                }
                
                _currentQuests.Add(newQuest);
            }
            OnGenerated?.Invoke();
           // _questUISetter.SetQuestUI(_currentQuests[i]);
        }
    }

    public void AddProgress(QuestData.QuestType questType, int progressAmount)
    {
        switch (questType)
        {
            case QuestData.QuestType.Kills:
                for (int i = 0; i < _currentQuests.Count; i++)
                {
                    if (_currentQuests[i].questType == QuestData.QuestType.Kills)
                    {
                        _currentQuests[i].questProgress += progressAmount;
                    }
                }
                break;
            case QuestData.QuestType.CurrencyWaste:
                for (int i = 0; i < _currentQuests.Count; i++)
                {
                    if (_currentQuests[i].questType == QuestData.QuestType.CurrencyWaste)
                    {
                        _currentQuests[i].questProgress += progressAmount;
                    }
                }
                break;
            case QuestData.QuestType.MissionComplete:
                for (int i = 0; i < _currentQuests.Count; i++)
                {
                    if (_currentQuests[i].questType == QuestData.QuestType.MissionComplete)
                    {
                        _currentQuests[i].questProgress += progressAmount;
                    }
                }
                break;
            case QuestData.QuestType.InGameTime:
                for (int i = 0; i < _currentQuests.Count; i++)
                {
                    if (_currentQuests[i].questType == QuestData.QuestType.InGameTime)
                    {
                        _currentQuests[i].questProgress += progressAmount;
                    }
                }
                break;
        }
    }
    
    public void UpdateProgress(int amount)
    {
        GenerateNewQuest();
    }

    public void GiveReward(QuestData questData)
    {
        for (int i = 0; i < questData.rewardTypes.Count; i++)
        {
            switch (questData.rewardTypes[i])
            {
                case QuestData.RewardType.Currency:
                    Debug.Log("Reward Currency: " + questData.currencyReward);
                    var startCur = PlayerPrefs.GetInt("money");
                    PlayerPrefs.SetInt("money", startCur + questData.currencyReward);
                    
                    break;
                case QuestData.RewardType.Experience:
                    Debug.Log("Reward Experience: " + questData.experienceReward);
                    var startXp = PlayerPrefs.GetInt("exp");
                    PlayerPrefs.SetInt("exp", startXp + questData.experienceReward);
                    break;
                case QuestData.RewardType.Item:
                    Debug.Log("Reward Item: " + questData.itemReward);
                    break;
            }
        }
    }
}
