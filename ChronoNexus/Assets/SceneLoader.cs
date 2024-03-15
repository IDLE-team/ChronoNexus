using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] private string _SceneToLoad;
    [SerializeField] private Image _blackLoadingImage;

    public void SceneToLoad()
    {
        StartCoroutine(Load());
        print(1);
    }

    public IEnumerator Load()
    {
        new WaitForSeconds(1f);

        LoadScene();
        yield return null;
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync(_SceneToLoad);
        
    }
}
