using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1f;
    static public LevelController instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void LoadSceneWithTransition(string sceneToLoad)
    {
        StartCoroutine(StartChangeSceneTransition(sceneToLoad));
    }

    public void LoadProcessWithTransition(float _preLoadTime,bool _isStart)
    {
        StartCoroutine(SetTransition( _preLoadTime,_isStart));
    }
    public void LoadScene(string sceneToLoad)
    {
        if(!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadSceneAsync(sceneToLoad);
    }
    private IEnumerator StartChangeSceneTransition(string sceneToLoad)
    {
        _transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(_transitionTime);

        LoadScene(sceneToLoad);
        yield return null;
    }
    private IEnumerator SetTransition(float _preLoadTime,bool _isStart)
    {
        yield return new WaitForSeconds(_preLoadTime);
        if (_isStart)
        {
            _transition.SetTrigger("Start");
        }
        else
        {
            _transition.SetTrigger("End");
        }
        
        
        yield return new WaitForSeconds(_transitionTime);

        yield return null;
    }
}