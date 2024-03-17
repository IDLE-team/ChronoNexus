using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject _startTab;

    private GameObject _currentTab;
    private GameObject _previousTab;

    [SerializeField] private float _duration;

    [SerializeField]
    private InterfaceAnimationComponent _anim;

    [SerializeField] private Image _black;

    private bool _isGame = false;

    private void Start()
    {
        _isGame = false;
        _currentTab = _startTab;
        OpenTab(_currentTab,_isGame);
    }

    public void OpenTab(GameObject tabToOpen, bool isGame)
    {
        _isGame = isGame;
        if (_isGame)
        {
            _black.gameObject.SetActive(false);
        }
        else
        {
            _black.gameObject.SetActive(true);
        }

        StartCoroutine(OpenTabCor(tabToOpen));
    }

    public void CloseTab(GameObject tabToClose)
    {
        StartCoroutine(CloseTabCor(tabToClose));
    }

    public void BackTab()
    {
        OpenTab(_previousTab,_isGame);
    }

    public IEnumerator OpenTabCor(GameObject tabToOpen)
    {
        tabToOpen.gameObject.SetActive(true);
        if (tabToOpen != _currentTab)
        {
            _previousTab = _currentTab ? _currentTab : null;

            _currentTab = tabToOpen;

            if (_previousTab != null)
            {
                CloseTab(_previousTab);
            }
            _anim.MoveUpOnScreen(_currentTab.GetComponent<RectTransform>(), _duration);
            yield return new WaitForSeconds(_duration);
        }
        
    }

    public IEnumerator CloseTabCor(GameObject tabToClose)
    {
        _anim.MoveDownOffScreen(tabToClose.GetComponent<RectTransform>(), _duration);
        yield return new WaitForSeconds(_duration);

        tabToClose.gameObject.SetActive(false);
    }
}
