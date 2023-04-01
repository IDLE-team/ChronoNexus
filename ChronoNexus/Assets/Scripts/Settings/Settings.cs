using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] [Min(30)] private int _frameRate = 60;
    [SerializeField] [Min(0)] private int _vSyncCount = 0;
    
    private void Start()
    {
        Application.targetFrameRate = _frameRate;
        QualitySettings.vSyncCount = _vSyncCount;
    }
}