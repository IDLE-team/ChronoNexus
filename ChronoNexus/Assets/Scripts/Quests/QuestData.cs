using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/New Quest")]
public class QuestData : ScriptableObject
{
    public string questName;
    public string requirements;
    
    public enum QuestType { Kills, InGameTime, CurrencyWaste, MissionComplete }
    public QuestType questType;
    public int questRequirments;
    
    public enum RewardType { Experience, Currency, Item, Both } 
    public List<RewardType> rewardTypes = new List<RewardType>(); 
    
    public int experienceReward; 
    public int currencyReward;
    public int itemReward;

    public int questProgress;

    public bool isComlete;

    public void ResetProgress()
    {
        questProgress = 0;
        isComlete = false;
    }
}
