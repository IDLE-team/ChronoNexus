using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    [SerializeField] private GameObject _startTab;
    [SerializeField] private GameObject _currentTab;
    [SerializeField] private GameObject _previousTab;


    private void Start()
    {
        _currentTab = _startTab;
        OpenTab(_currentTab);
    }

    public void OpenTab(GameObject tabToOpen)
    {
        if (tabToOpen != _currentTab)
        {
            _previousTab = _currentTab ? _currentTab : null;

            tabToOpen.SetActive(true);
            _currentTab = tabToOpen;
           // print(_currentTab.activeSelf + _currentTab.name);

            if (_previousTab != null)
            {
                _previousTab.SetActive(false);
             //   print(_previousTab.activeSelf + _previousTab.name);
            }
        }
    }

    public void BackTab()
    {
        //print("Back Tab " + _previousTab.name);
        OpenTab(_previousTab);
    }
}
