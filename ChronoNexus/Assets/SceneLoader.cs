using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private string _SceneToLoad;

    public void SceneToLoad()
    {
        StartCoroutine(Load());
    }

    public IEnumerator Load()
    {
        new WaitForSeconds(1f);

        LoadScene();
        yield return null;
    }

    private void LoadScene()
    {
        Debug.LogError(_SceneToLoad);
        SceneManager.LoadSceneAsync(_SceneToLoad);
    }
}
