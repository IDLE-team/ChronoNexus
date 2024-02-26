using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugInfoUI : MonoBehaviour
{
    [SerializeField] private СharacterTargetingSystem _targetLock;
    private TextMeshProUGUI _debugText;

    private void Start()
    {
        _debugText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _debugText.text =
            $"LookTarget: {(_targetLock.Target != null ? _targetLock.Target.GetTransform().gameObject.name : "null")}\n" +
            $"PreviousTarget: {(_targetLock.PreviousTarget != null ? _targetLock.PreviousTarget.GetTransform().gameObject.name : "null")}\n" +

            $"\nisLookAt: {_targetLock.IsLookAt}\n" +
            $"isEnemyTargeted: {_targetLock.IsEnemyTargeted}\n" +
            $"isStickSearch: {_targetLock.IsStickSearch} \n" +

            $"\nRefreshTargetAsync: {_targetLock.DebugTestUniTask} \n";
    }
}