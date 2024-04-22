using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetData : MonoBehaviour
{
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll(); 
        Restart();
    }

}
