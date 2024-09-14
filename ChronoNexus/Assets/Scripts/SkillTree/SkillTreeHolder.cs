using System.Collections.Generic;
using UnityEngine;

public class SkillTreeHolder : MonoBehaviour
{

    [SerializeField] private SkillTreeType _type;

    [SerializeField] private List<SkillScriptableObject> _skills;
    [SerializeField] private GameObject _skillHolder;

    void Start()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            var skill = Instantiate(_skillHolder, transform);
            skill.GetComponent<SkillHolder>().InitializeSkillVisual(_skills[i]);
        }
    }

}

public enum SkillTreeType
{
    attack, defence, time
}


