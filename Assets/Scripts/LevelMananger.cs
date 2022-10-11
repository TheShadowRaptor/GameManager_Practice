using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMananger : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
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
