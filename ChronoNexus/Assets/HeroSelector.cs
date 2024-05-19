using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelector : MonoBehaviour
{
    public CharacterData hero;
    Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SetHeroId);
    }

    private void SetHeroId()
    {
        PlayerPrefs.SetInt("charID", HubIventoryManager.manager.GetHeroIdByData(hero));
    }
}
