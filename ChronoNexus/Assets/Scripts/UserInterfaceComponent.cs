using Zenject;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceComponent : MonoBehaviour
{
    [SerializeField] private UserInterfaceManager _manager;
    [SerializeField] private Button _backButton;

    private void OnDrawGizmos()
    {
        _manager = GetComponentInParent<UserInterfaceManager>();
    }

    private void OnEnable()
    {
        if (_manager)
        {
            _manager.OpenTab(gameObject);
        }
    }

    private void OnDisable()
    {
        if (_manager)
        {
            _manager.CloseTab(gameObject);
        }
    }

    private void Start()
    {
        if (!_manager && _backButton)
        {
            _manager = GetComponentInParent<UserInterfaceManager>();
        }

        
        _backButton?.onClick.AddListener(_manager.BackTab);
    }
}
