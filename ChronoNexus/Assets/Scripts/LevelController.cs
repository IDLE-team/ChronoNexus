using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
}