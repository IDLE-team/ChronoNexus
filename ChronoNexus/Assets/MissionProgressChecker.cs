using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionProgressChecker : MonoBehaviour
{
    [SerializeField] private LevelStatTracker _levelStatTracker;
    private DailyQuestSystem _questSystem;

    private void Start()
    {
        _questSystem = GameObject.Find("QuestController").GetComponent<DailyQuestSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckQuestProgress();
        }
    }

    public void CheckQuestProgress()
    {
        for (int i = 0; i < _questSystem.CurrentQuests.Count; i++)
        {
            switch (_questSystem.CurrentQuests[i].questType)
            {
                case QuestData.QuestType.Kills:
                    _questSystem.CurrentQuests[i].questProgress += _levelStatTracker.GetKilledEnemyAmount();
                    if (_questSystem.CurrentQuests[i].questProgress >= _questSystem.CurrentQuests[i].questRequirments)
                    {
                        _questSystem.CurrentQuests[i].questProgress = _questSystem.CurrentQuests[i].questRequirments;
                        _questSystem.GiveReward(_questSystem.CurrentQuests[i]);
                        _questSystem.CurrentQuests[i].isComlete = true;
                    }
                    Debug.Log("Зачли прогресс: " +   _questSystem.CurrentQuests[i].questProgress );
                    break;
            }
            
        }
    }
}
