using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUISetter : MonoBehaviour
{
    [SerializeField] private GameObject _questHolderPrefab;
    public void SetQuestUI(QuestData questData)
    {
        Debug.Log("Set");
        var quest = Instantiate(_questHolderPrefab, gameObject.transform);
        quest.GetComponent<QuestUI>().SetData(questData);
    }
}
