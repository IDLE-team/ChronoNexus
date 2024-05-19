using UnityEngine;
using UnityEngine.UI;

public class CharacterHolder : MonoBehaviour
{
    private Image _charImage;

    private int _character;


    private void Awake()
    {
        _charImage = GetComponent<Image>();
    }


    private void OnEnable()
    {
        SetHero();
    }

    private void Start()
    {
        SetHero();
    }

    public void SetHero()
    {
        _character = PlayerPrefs.GetInt("charID", 0);
        CharacterData data;

        if (HubIventoryManager.manager)
        {
            data = HubIventoryManager.manager.SelectedHeroData(_character);
            _charImage.sprite = data.heroImage;
        }
    }
}
