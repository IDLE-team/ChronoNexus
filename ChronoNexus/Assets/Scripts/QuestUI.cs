using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _requirments;
    [SerializeField] private TextMeshProUGUI _requirmentsProgress;
    [SerializeField] private TextMeshProUGUI _expReward;
    [SerializeField] private TextMeshProUGUI _curReward;
    [SerializeField] private TextMeshProUGUI _itemReward;

    [SerializeField] private Slider _progressSlider;

    [SerializeField] private GameObject _completeScreen;
    
    [SerializeField] private GameObject _expHolder;
    [SerializeField] private GameObject _curHolder;
    [SerializeField] private GameObject _itemHolder;
    
    private QuestData _questData;
    
    public void SetData(QuestData questData)
    {
        _questData = questData;
        
        _name.text = questData.questName;
        _requirments.text = questData.requirements;
        _requirmentsProgress.text = questData.questProgress + " / " + questData.questRequirments;
        _progressSlider.maxValue = questData.questRequirments;
        _progressSlider.value = questData.questProgress;

        if (questData.isComlete)
        {
            _completeScreen.SetActive(true);
        }
        
        
        for (int i = 0; i < _questData.rewardTypes.Count; i++)
        {
            switch (_questData.rewardTypes[i])
            {
                case QuestData.RewardType.Currency:
                    _curHolder.SetActive(true);
                    _curReward.text = _questData.currencyReward.ToString();
                    break;
                case QuestData.RewardType.Experience:
                    _expHolder.SetActive(true);
                    _expReward.text = _questData.experienceReward.ToString();
                    break;
                case QuestData.RewardType.Item:
                    _itemHolder.SetActive(true);
                    _itemReward.text = _questData.itemReward.ToString();
                    break;
            }
                
        }
    }
}
