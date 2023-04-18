using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private TextMeshProUGUI _enemyCounter;
    [SerializeField] private Slider enemySlider;
    [SerializeField] private TMP_Dropdown enemyDropdown;
    [SerializeField] private int maxEnemies = 20;

    public List<GameObject> enemyList = new List<GameObject>();

    private Vector3 _spawnPosition;
    private int currentEnemies => enemyList.Count;

    private void Start()
    {
        enemySlider.maxValue = maxEnemies;
        _spawnPosition = transform.position;
        enemySlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void SetEnemyCount()
    {
        if ((int)enemySlider.value > enemyList.Count)
        {
            SpawnEnemies((int)enemySlider.value - enemyList.Count);
        }
        else if ((int)enemySlider.value < enemyList.Count)
        {
            DestroyEnemies(enemyList.Count - (int)enemySlider.value);
        }
    }

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (enemyList.Count < 10)
            {
                _spawnPosition = new Vector3(transform.position.x + enemyList.Count + 0.5f, transform.position.y, transform.position.z);
            }
            else if (enemyList.Count >= 10 && enemyList.Count < 20)
            {
                _spawnPosition = new Vector3(transform.position.x + enemyList.Count - 9.5f, transform.position.y, transform.position.z + 2f);
            }
            else if (enemyList.Count >= 20 && enemyList.Count < 30)
            {
                _spawnPosition = new Vector3(transform.position.x + enemyList.Count - 19.5f, transform.position.y, transform.position.z + 4f);
            }
            else if (enemyList.Count >= 30 && enemyList.Count < 40)
            {
                _spawnPosition = new Vector3(transform.position.x + enemyList.Count - 29.5f, transform.position.y, transform.position.z + 6f);
            }
            else if (enemyList.Count >= 40 && enemyList.Count < 50)
            {
                _spawnPosition = new Vector3(transform.position.x + enemyList.Count - 39.5f, transform.position.y, transform.position.z + 8f);
            }

            var enemy = Instantiate(enemyPrefab, _spawnPosition, Quaternion.identity);
            enemy.name = "Enemy " + i;
            var enemyScript = enemy.GetComponent<Enemy>();
            switch (enemyDropdown.value)
            {
                case 0:
                    enemyScript.InitializeSpawner(this, enemyScript.DummyState);
                    break;

                case 1:
                    enemyScript.InitializeSpawner(this, enemyScript.IdleState);
                    break;

                case 2:
                    enemyScript.InitializeSpawner(this, enemyScript.PatrolState);
                    break;
            }

            enemyList.Add(enemy);
        }
    }

    private void DestroyEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Destroy(enemyList[enemyList.Count - 1]);
            enemyList.RemoveAt(enemyList.Count - 1);
        }
    }

    public void DestroyEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy);
        enemySlider.value = enemyList.Count;
    }

    private void OnSliderValueChanged(float value)
    {
        _enemyCounter.text = value.ToString();
    }
}