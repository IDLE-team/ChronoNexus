using UnityEngine;
using UnityEngine.UI;

public class CharcterSkinHolder : MonoBehaviour
{

    private Image _image;

    private SkinData _skinData;
    private void Start()
    {
        _image = gameObject.GetComponent<Image>();
        PlayerProfileManager.profile.heroChanged += SkinChanged;

        SkinChanged();
    }

    private void SkinChanged()
    {
        _skinData = SkinDataManager.skinManager.GetSkinDataByIndex(PlayerPrefs.GetInt("hero", 0));
        _image.sprite = _skinData.charSprite;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("hero", SkinDataManager.skinManager.GetIndexBySkinData(_skinData));
        PlayerProfileManager.profile.heroChanged -= SkinChanged;
    }

}
