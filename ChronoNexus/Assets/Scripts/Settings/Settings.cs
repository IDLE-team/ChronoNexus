using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField][Min(30)] private int _frameRate = 60;
    [SerializeField][Min(0)] private int _vSyncCount = 0;
    [SerializeField] private TMP_Dropdown _dropdown;

    [SerializeField] private bool _isLocked;
    private void Start()
    {
        if(!_isLocked)
            return;
        _frameRate =  PlayerPrefs.GetInt("fps", 60);
        Debug.Log("FrameRate: " + _frameRate);
        Application.targetFrameRate = _frameRate;
        QualitySettings.vSyncCount = _vSyncCount;
        Debug.Log("TargetFPS: " +   Application.targetFrameRate);

        // Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    public void ChangeMaxFps()
    {
        switch (_dropdown.value)
        {
            case 0:
                _frameRate = 30;
                break;

            case 1:
                _frameRate = 60;
                break;

            case 2:
                _frameRate = 90;
                break;

            case 3:
                _frameRate = 120;
                break;

            default:
                _frameRate = 60;
                break;
        }
        PlayerPrefs.SetInt("fps", _frameRate);
        Debug.Log("PlayerPrefs setted: " + PlayerPrefs.GetInt("fps"));
        Application.targetFrameRate = _frameRate;
    }
}