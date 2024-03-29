using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject _lockScreen;
    [SerializeField] private Toggle _levelToggleButton;
    [SerializeField] private SceneLoader _sceneLoader;

    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private int _kills;
    [SerializeField] private TimeSpan _time;
    [SerializeField] private bool _cleared;

    [SerializeField] private string _levelName;
    [SerializeField] private string _requiredLevelName;

    [SerializeField] private bool _isLocked = true;


    private void OnEnable()
    {
        LoadData(_levelName);
        _levelToggleButton.onValueChanged.AddListener(OnToggleValueChanged);
        
        if (!_isLocked)
        {
            Unlock();
            return;
        }
        if (!string.IsNullOrEmpty(_requiredLevelName))
        {
            Debug.Log("TryOpen, required: " + _requiredLevelName);
            Debug.Log("GetCleared: " + GetRequiredLevelCleared());
            if(GetRequiredLevelCleared())
                Unlock();
        }
    }

    private void OnDisable()
    {
        _levelToggleButton.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            SelectLevel();
        }
    }

    private void SelectLevel()
    {
        _sceneLoader.SetScene(_levelName);
    }

    public bool GetRequiredLevelCleared()
    {
        string data = PlayerPrefs.GetString(_requiredLevelName);

        if (!string.IsNullOrEmpty(data))
        {
            Debug.Log("Не Numll");
            string[] parts = data.Split(';');

            foreach (string part in parts)
            {

                    string[] keyValue = part.Split('-');

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        Debug.Log("Перед проверкой ключа");
                        if (key == "Cleared")
                        {
                            bool cleared = bool.Parse(value);
                            Debug.Log("Last level cleared: " + cleared);
                            return cleared;
                        }
                    }
            }
        }

        return false;
    }
    public void LoadData(string levelName)
    {
        string data = PlayerPrefs.GetString(levelName);
        Debug.Log(data);
        if (!string.IsNullOrEmpty(data))
        {
            string[] parts = data.Split(';');

            foreach (string part in parts)
            {
                string[] keyValue = part.Split('-');

                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();

                    switch (key)
                    {
                        case "Kills":
                             _kills = int.Parse(value);
                             _killsText.text = "Убийств\n" + _kills;
                            Debug.Log("Kills: " + _kills);
                            break;
                        case "Time":
                            TimeSpan time = TimeSpan.ParseExact(value, @"mm\:ss\:ff", CultureInfo.InvariantCulture);
                            _timeText.text = "Время\n" + time.ToString(@"mm\:ss\:ff");
                            Debug.Log("Time: " + time.ToString(@"mm\:ss\:ff"));
                            break;
                        case "Cleared":
                            _cleared = bool.Parse(value);
                            Debug.Log("Cleared: " + _cleared);
                            break;
                    }
                }
            }
        }

    }
    /*
    public void LoadData(string levelName)
    {
        string data = PlayerPrefs.GetString(levelName);
        if (data == null)
            return;

        string[] dataArray = data.Split(';');

        bool cleared = false;
        int kills = 0;
        float time = 0;

        foreach (string dataString in dataArray)
        {
            if (dataString.Contains("Kills"))
            {
                kills = int.Parse(dataString.Split(':')[1].Trim());
            }
            else if (dataString.Contains("Time"))
            {
                time = float.Parse(dataString.Split(':')[1].Trim().Split(':')[0]) * 3600 +
                       float.Parse(dataString.Split(':')[1].Trim().Split(':')[1]) * 60 +
                       float.Parse(dataString.Split(':')[1].Trim().Split(':')[2]);
            }
            else if (dataString.Contains("Cleared"))
            {
                cleared = bool.Parse(dataString.Split(':')[1].Trim());
            }
        }

        if (cleared)
        {
            Debug.Log("Level cleared!");
            Debug.Log("Kills: " + kills);
            Debug.Log("Time: " + TimeSpan.FromSeconds(time).ToString(@"hh\:mm\:ss"));
        }
        else
        {
            Debug.Log("Level not cleared.");
        }

        
        
        _cleared = cleared;
        _kills = kills;
        _time = time;
    
    }
*/
    /*
    private void SetTextStats()
    {
        _killsText.text = "Убийств\n" + _kills;
        _timeText.text = "Время\n" +  TimeSpan.FromSeconds(_time).ToString(@"hh\:mm\:ss");
    }
    */
private void Unlock()
    {          
        _isLocked = false;
        _levelToggleButton.interactable = true;
        if(!_lockScreen)
            return;
        _lockScreen.SetActive(false);
    }
}
