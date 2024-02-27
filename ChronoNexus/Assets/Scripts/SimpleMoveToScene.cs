using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleMoveToScene : MonoBehaviour
{
    [SerializeField] private string _sceneToMoveTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MoveToScene();
        }
    }
    public void MoveToScene()
    {
        SceneManager.LoadScene(_sceneToMoveTo);
    }
}
