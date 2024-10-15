using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skills", order = 1)]
public class SkillScriptableObject : ScriptableObject
{

    public SkillTreeType type;
    public UpgradeType upgradeType;
    public string skillName;
    [TextArea]
    public string skillDescription; // base skill description

    public string progressValueText;
    public Color _progressValueTextColor;

    public int currentLvl = 0;
    public int maxLvl = 3;
    public List<int> upgradeValuePerLevel = new List<int>();
    public string[] levelDescription; // level parameters with colored parts
    public Sprite skillIconImage;


    public Color ReturnColorByType(SkillTreeType type)
    {
        string hex;
        Color newCol;
        switch (type)
        {
            case SkillTreeType.time:
                hex = "#7C7CFE";
                break;

            case SkillTreeType.attack:
                hex = "#FE7C8E";
                break;

            case SkillTreeType.defence:
                hex = "#03FAA2";
                break;

            default:
                hex = "#000000";
                break;
        }
        ColorUtility.TryParseHtmlString(hex, out newCol);
        return newCol;
    }

    public int GetUpgradeValue()
    {
        Debug.Log("Upgr: " + upgradeValuePerLevel.Count);
        Debug.Log("CurLvl: " + currentLvl);
        return upgradeValuePerLevel[currentLvl-1];
    }
}
