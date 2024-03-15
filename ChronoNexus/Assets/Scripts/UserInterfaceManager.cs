using System.Collections;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject _startTab;

    private GameObject _currentTab;
    private GameObject _previousTab;

    [SerializeField] private float _duration;

    [SerializeField]
    private InterfaceAnimationComponent _anim;

    private void Start()
    {
        _currentTab = _startTab;
        OpenTab(_currentTab);
    }

    public void OpenTab(GameObject tabToOpen)
    {
        StartCoroutine(OpenTabCor(tabToOpen));
    }

    public void CloseTab(GameObject tabToClose)
    {
        StartCoroutine(CloseTabCor(tabToClose));
    }

    public void BackTab()
    {
        OpenTab(_previousTab);
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
