using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneStringName;
    public string levelDescription;
    public Sprite levelSprite;

    public int difficulty = 1;
    public int moneyReward = 0;
    public int expReward = 0;
    public int materialReward = 0;
    public int gunRewardID = -1; //-1 is no gun, >=0 is some gun ID
}
