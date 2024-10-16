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
        _questSystem = DailyQuestSystem.Instance;
    }

    public void CheckQuestProgress()
    {
        for (int i = 0; i < _questSystem.CurrentQuests.Count; i++)
        {
            Debug.Log("PreSwitch: " + _questSystem.CurrentQuests[i].questType);
            switch (_questSystem.CurrentQuests[i].questType)
            {
                case QuestData.QuestType.Kills:
                    Debug.Log("InQuest");
                    _questSystem.CurrentQuests[i].questProgress += _levelStatTracker.GetKilledEnemyAmount();
                    if (_questSystem.CurrentQuests[i].questProgress >= _questSystem.CurrentQuests[i].questRequirments)
                    {
                        _questSystem.CurrentQuests[i].questProgress = _questSystem.CurrentQuests[i].questRequirments;
                        _questSystem.GiveReward(_questSystem.CurrentQuests[i]);
                        _questSystem.CurrentQuests[i].isComlete = true;
                    }
                    Debug.Log("Зачли прогресс: " +   _questSystem.CurrentQuests[i].questProgress );
                    break;
                default:
                    Debug.Log("Default");
            }
            
        }
    }
}
