using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateUIUpdater : MonoBehaviour
{
    [SerializeField] private Entity _entity;
    [SerializeField] TextMeshProUGUI _textMesh;
    private Camera _cam;
    private void OnEnable()
    {
        _entity.OnUIUpdate += UpdateStateUI;
    }
    private void OnDisable()
    {
        _entity.OnUIUpdate -= UpdateStateUI;
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
        //_textMesh.text = _entity.CurrentState.ToUpper();
    }
}
