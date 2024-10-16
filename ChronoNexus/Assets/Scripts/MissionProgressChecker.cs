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
       // _questSystem = DailyQuestSystem.Instance;
    }

    public void CheckQuestProgress()
    {
        _questSystem = DailyQuestSystem.Instance;

        Debug.Log("Count: " + _questSystem.CurrentQuests.Count);
        for (int i = 0; i < _questSystem.CurrentQuests.Count; i++)
        {
            Debug.Log("CurrentQuestCheck: " + _questSystem.CurrentQuests[i].questType);
            switch (_questSystem.CurrentQuests[i].questType)
            {
                case QuestData.QuestType.Kills:
                    Debug.Log("KillsQuest");
                    _questSystem.CurrentQuests[i].questProgress += _levelStatTracker.GetKilledEnemyAmount();
                    CheckCompleteQuest(i);
                    Debug.Log("Зачли прогресс киллы: " + _questSystem.CurrentQuests[i].questProgress);
                    break;

                case QuestData.QuestType.MissionComplete:
                    Debug.Log("MissonsCompleteQuest");
                    if (!_levelStatTracker.GetLevelCleared())
                    {
                        Debug.Log("NoCleared: " + _levelStatTracker.GetLevelCleared());
                        return;
                    }
                    _questSystem.CurrentQuests[i].questProgress++;
                    CheckCompleteQuest(i);
                    Debug.Log("Зачли прогресс миссии: " + _questSystem.CurrentQuests[i].questProgress);
                    break;
                default:
                    break;
            }
        }
    }

    public void CheckCompleteQuest(int questID)
    {
        if (_questSystem.CurrentQuests[questID].questProgress >= _questSystem.CurrentQuests[questID].questRequirments)
        {
            CompleteQuest(questID);
        }
    }

    public void CompleteQuest(int questID)
    {
        _questSystem.CurrentQuests[questID].questProgress = _questSystem.CurrentQuests[questID].questRequirments;
        _questSystem.GiveReward(_questSystem.CurrentQuests[questID]);
        _questSystem.CurrentQuests[questID].isComlete = true;
    }
}