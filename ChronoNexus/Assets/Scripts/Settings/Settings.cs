using UnityEngine;

public class Settings : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
}