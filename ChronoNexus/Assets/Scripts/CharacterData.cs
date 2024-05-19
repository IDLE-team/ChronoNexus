using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Hero", order = 1)]
public class CharacterData : ScriptableObject
{
    public string heroName;
    public Sprite heroImage;

    //public SpellType type                -- for abilities
}