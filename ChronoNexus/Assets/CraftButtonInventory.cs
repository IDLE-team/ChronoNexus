using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class CraftButtonInventory : MonoBehaviour
{
    [SerializeField] private GameObject _upgradeTab;
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Color _colorPressed;

    private void Start()
    {
        _button.onClick.AddListener(ChangeTabState);
    }

    private void OnEnable()
    {
        if (_upgradeTab.activeSelf)
        {
            _buttonImage.color = Color.white;
            _upgradeTab.SetActive(false);
        }

        PlayerProfileManager.profile.itemChanged();
    }

    public void ChangeTabState()
    {
        if (!_upgradeTab.activeSelf)
        {
            _upgradeTab.SetActive(true);
            _buttonImage.color = _colorPressed;
            _upgradeTab.transform.localScale = Vector3.zero;
            _upgradeTab.transform.DOScale(Vector3.one,0.25f);
            
        }
        else
        {
            _buttonImage.color = Color.white;
            DelayShut();
        }
    }

    public void ShutTabImediat()
    {
        _upgradeTab.SetActive(false);
        _buttonImage.color = Color.white;
    }

    private IEnumerator DelayShutCor()
    {
        _upgradeTab.transform.localScale = Vector3.one;
        _upgradeTab.transform.DOScale(Vector3.zero, 0.25f);
        _buttonImage.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        _upgradeTab.SetActive(false);
    }

    public void DelayShut()
    {
        StartCoroutine(DelayShutCor());
    }

}
