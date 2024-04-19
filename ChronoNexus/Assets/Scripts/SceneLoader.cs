using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private string _sceneToLoad;
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1f;
    [SerializeField] private string _currentLevelName;
    [SerializeField] private LevelStatTracker _levelStatTracker;
    [SerializeField] private WinScreen _winScreen;
    private void Start()
    {
        _currentLevelName = SceneManager.GetActiveScene().name;
        _levelStatTracker = gameObject.GetComponent<LevelStatTracker>();
        _winScreen = FindFirstObjectByType<WinScreen>();
    }

    public void SetScene(string scene)
    {
        _sceneToLoad = scene;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            //PlayerPrefs.SetInt(_currentLevelName, 1);
            SaveLevelData();
            if (_winScreen != null)
                _winScreen.SetScreen(_levelStatTracker,this);

        }
    }
    public void SaveLevelData()
    {
        int kills = _levelStatTracker.GetKilledEnemyAmount();
        float time = _levelStatTracker.GetLevelWalkthroughTime();
        string data = "Kills- " + kills + "; Time- " + TimeSpan.FromSeconds(time).ToString(@"mm\:ss\:ff") + "; Cleared- " + true;
        PlayerPrefs.SetString(_currentLevelName, data);
    }
    public void SceneToLoad()
    {
        StartCoroutine(Load());
    }

    public IEnumerator Load()
    {
        _transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(_transitionTime);

        LoadScene();
        yield return null;
    }

    private void LoadScene()
    {
        if(_sceneToLoad != null &&  _sceneToLoad != "" && _sceneToLoad != " " )
            SceneManager.LoadSceneAsync(_sceneToLoad);
    }
}
