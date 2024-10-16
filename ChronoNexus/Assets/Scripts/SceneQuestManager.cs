using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneQuestManager : MonoBehaviour
{
   private DailyQuestSystem _dailyQuestSystem;
   private bool _trackKills;
   private bool _trackWaste;
   private bool _trackMissionComplete;
   private bool _trackInGameTime;

   public bool TrackKills => _trackKills;
   public bool TrackWaste => _trackWaste;
   public bool TrackMissionComplete => _trackMissionComplete;
   public bool TrackInGameTime => _trackInGameTime;
   
   private void Start()
   {
      _dailyQuestSystem = DailyQuestSystem.Instance;
      SetTrackers();
   }

   private void SetTrackers()
   {
      for (int i = 0; i < _dailyQuestSystem.CurrentQuests.Count; i++)
      {
         switch (_dailyQuestSystem.CurrentQuests[i].questType)
         {
            case QuestData.QuestType.Kills:
               TrackKillQuestProgress();
               break;
            case QuestData.QuestType.CurrencyWaste:
               TrackWasteQuestProgress();
               break;
            case QuestData.QuestType.MissionComplete:
               TrackMissionCompleteQuestProgress();
               break;
            case QuestData.QuestType.InGameTime:
               TrackInGameTimeQuestProgress();
               break;
         }
      }
   }
   
   private void TrackKillQuestProgress()
   {
      _trackKills = true;
   }
   
   private void TrackWasteQuestProgress()
   {
      _trackWaste = true;
   }
   
   private void TrackMissionCompleteQuestProgress()
   {
      _trackMissionComplete = true;
   }
   
   private void TrackInGameTimeQuestProgress()
   {
      _trackInGameTime = true;
   }

   public void AddProgress(QuestData.QuestType questType, int progressAmount)
   {
      _dailyQuestSystem.AddProgress(questType, progressAmount);
   }
}
