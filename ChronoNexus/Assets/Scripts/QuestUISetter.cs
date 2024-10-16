using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUISetter : MonoBehaviour
{
    [SerializeField] private GameObject _questHolderPrefab;
    private bool _initialized;
    private void OnEnable()
    {
        DailyQuestSystem.Instance.OnGenerated += Initialize;
    }

    private void OnDisable()
    {
        DailyQuestSystem.Instance.OnGenerated -= Initialize;

    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if(_initialized)
            return;
        if (DailyQuestSystem.Instance.CurrentQuests.Count > 0)
        {
            for (int i = 0; i < DailyQuestSystem.Instance.CurrentQuests.Count; i++)
            {
                SetQuestUI(DailyQuestSystem.Instance.CurrentQuests[i]);
            }

            _initialized = true;
        }
    }
    public void SetQuestUI(QuestData questData)
    {
        var quest = Instantiate(_questHolderPrefab, gameObject.transform);
        quest.GetComponent<QuestUI>().SetData(questData);
    }
}
