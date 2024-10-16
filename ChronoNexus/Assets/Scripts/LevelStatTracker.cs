using UnityEngine;

public class LevelStatTracker : MonoBehaviour
{
    [SerializeField] private float _startTime;
    [SerializeField] private float _currentTime;

    [SerializeField] private int _startEnemyAmount;

    [SerializeField] private int _kills;
    [SerializeField] private int _materialsCount;
    [SerializeField] private bool _isLevelCleared;
    private void Start()
    {
        _startTime = Time.realtimeSinceStartup;
        _startEnemyAmount = Entity.enemyList.Count;
        Debug.Log("StartEnAm: " + _startEnemyAmount);
    }

    public void SetLevelCleared()
    {
        _isLevelCleared = true;
    }
    public bool GetLevelCleared()
    {
        return _isLevelCleared;
    }
    public int GetMaterialCount()
    {
        return _materialsCount;
    }

    public float GetLevelWalkthroughTime()
    {
        return Time.realtimeSinceStartup - _startTime;
    }

    public int GetKilledEnemyAmount()
    {
        Debug.Log("StartEn: " + _startEnemyAmount + " - " + "Now: " + Entity.enemyList.Count);
        return _startEnemyAmount - Entity.enemyList.Count;
    }
    

    public void AddMaterials(int count)
    {
        _materialsCount += count;
    }
}
