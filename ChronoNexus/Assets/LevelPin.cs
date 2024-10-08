using UnityEngine;
using UnityEngine.UI;

public class LevelPin : MonoBehaviour
{
    [SerializeField] private LevelData _level;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private LevelDescriptionHolder _levelDescriptionHolder;

    private Toggle _levelToggle;

    private void Start()
    {
        _levelIcon.sprite = _level.levelSprite;

        _levelToggle = gameObject.GetComponent<Toggle>();

    }

    public void SetDisplay()
    {
        if (_levelToggle)
        {
           if (_levelToggle.isOn)  _levelDescriptionHolder.DisplayData(_level);
        }
    }

    public void SetLevelData(LevelData level)
    {
        _level = level;
    }
}
