using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    public Action OnGenerated;
    
    private int progress;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        transform.SetParent(null);
    }

    void Start()
    {
        GenerateNewQuest();
    }

    void GenerateNewQuest()
    {
        for (int i = 0; i < _maxQuestsPerDay; i++)
        {
            Debug.Log("i: " + i);
            Debug.Log("quests.Count: " + quests.Count);

            if (i > quests.Count-1)
            {
                Debug.Log("Break");
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
            _questUISetter.SetQuestUI(_currentQuests[i]);
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

        GiveReward();
        GenerateNewQuest();
    }

    void GiveReward()
    {
    }
}
