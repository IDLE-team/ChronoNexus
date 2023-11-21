using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateUIUpdater : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] TextMeshProUGUI _textMesh;
    private Camera _cam;
    private void OnEnable()
    {
        _enemy.OnUIUpdate += UpdateStateUI;
    }
    private void OnDisable()
    {
        _enemy.OnUIUpdate -= UpdateStateUI;
    }
    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
    private void UpdateStateUI()
    {
        _textMesh.text = _enemy.CurrentState.ToUpper();
    }
}
