using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMananger : MonoBehaviour
{
    public void LoadLevel(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void LoadMainMenu() 
    { 
        SceneManager.LoadScene(0); 
    }
}
