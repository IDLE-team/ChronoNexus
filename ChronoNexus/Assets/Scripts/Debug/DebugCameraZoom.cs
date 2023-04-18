using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class DebugCameraZoom : MonoBehaviour
{
    [SerializeField] private Slider _cameraZoonSlider;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private TextMeshProUGUI _yText;
    [SerializeField] private TextMeshProUGUI _zText;

    private float _yOffset = 6.59f;
    private float _zOffset = -7.16f;
    private Vector3 _baseOffset;
    private Vector3 _followOffset;

    private void OnEnable()
    {
        _cameraZoonSlider.onValueChanged.AddListener(SetCameraZoom);
        _yText.text = $"Y: {_baseOffset.y}";
        _zText.text = $"Z: {_baseOffset.z}";
    }

    private void Start()
    {
        _followOffset = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        _baseOffset = new Vector3(0, _yOffset, _zOffset);
    }

    private void SetCameraZoom(float value)
    {
        _followOffset = new Vector3(_baseOffset.x, _baseOffset.y * -value, _baseOffset.z * -value);
        _virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _followOffset;
        _yText.text = $"Y: {_followOffset.y}";
        _zText.text = $"Z: {_followOffset.z}";
    }
}