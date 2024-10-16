using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelPin : MonoBehaviour
{
    [SerializeField] private LevelData _level;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private LevelDescriptionHolder _levelDescriptionHolder;

    [SerializeField]
    private bool _isActivated;
    private Toggle _levelToggle;

    private void OnEnable()
    {
        if (_levelToggle)
        {
            SetDisplay(_isActivated);
        }
    }

    private void Start()
    {
        _levelIcon.sprite = _level.levelSprite;

        _levelToggle = gameObject.GetComponent<Toggle>();
    }

    public void SetDisplay(bool isActive)
    {
        
        _isActivated = isActive;
        if (isActive && _isActivated )
        {
            _levelDescriptionHolder.DisplayData(_level);
        }
    }

    public void SetLevelData(LevelData level)
    {
        _level = level;
    }
}