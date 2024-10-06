using UnityEngine;
using UnityEngine.UI;

public class LevelPin : MonoBehaviour
{
    [SerializeField] private LevelData _level;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private LevelDescriptionHolder _levelDescriptionHolder;

    private void Start()
    {
        _levelIcon.sprite = _level.levelSprite;
    }

    public void SetDisplay()
    {
        _levelDescriptionHolder.DisplayData(_level);
    }

    public void SetLevelData(LevelData level)
    {
        _level = level;
    }
}
