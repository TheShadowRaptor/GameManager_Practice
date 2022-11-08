using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIMananger uIMananger;
    public LevelMananger levelMananger;

    GameObject playerObj;
    FirstPersonController_Sam player;
    public enum GameState 
    {
        mainMenu,
        settings,
        gameplay,
        pause,
        credits,
        gameOver,
        win
    }

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.mainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            playerObj = GameObject.Find("Player");
            player = playerObj.GetComponent<FirstPersonController_Sam>();
        }

        switch (gameState)
        {
            case GameState.mainMenu:
                Time.timeScale = 0;
                player.cursorLocked = false;
                player.canMove = false;
                uIMananger.MainMenuCanvasOn();
                break;

            case GameState.settings:
                Time.timeScale = 0;
                player.cursorLocked = false;
                player.canMove = false;
                uIMananger.SettingsCanvasOn();
                break;

            case GameState.gameplay:
                Time.timeScale = 1;
                player.cursorLocked = true;
                player.canMove = true;
                uIMananger.GameplayCanvasOn();
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    gameState = GameState.pause;
                }
                if (Input.GetKey(KeyCode.U))
                {
                    gameState = GameState.win;
                }
                if (Input.GetKey(KeyCode.I))
                {
                    gameState = GameState.gameOver;
                }
                break;

            case GameState.pause:
                Time.timeScale = 0;
                player.cursorLocked = false;
                player.canMove = false;
                uIMananger.PauseCanvasOn();
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    gameState = GameState.gameplay;
                }
                break;

            case GameState.credits:
                Time.timeScale = 0;
                player.cursorLocked = false;
                player.canMove = false;
                uIMananger.CreditsCanvasOn();
                break;

            case GameState.gameOver:
                Time.timeScale = 0;
                player.cursorLocked = false;
                player.canMove = false;
                uIMananger.GameOverCanvasOn();
                break;

            case GameState.win:
                Time.timeScale = 0;
                player.cursorLocked = false;
                player.canMove = false;
                uIMananger.WinCanvasOn();
                break;
        }
    }

    public void LoadSettings()
    {
        gameState = GameState.settings;
    }
    public void Pause()
    {
        gameState = GameState.pause;
    }
    public void UnPause()
    {
        gameState = GameState.gameplay;
    }

    public void LoadGameplay()
    {
        levelMananger.LoadLevel();
        gameState = GameState.gameplay;
    }

    public void LoadMainMenu()
    {
        levelMananger.LoadMainMenu();
        gameState = GameState.mainMenu;
    }

    public void EndGame()
    {
        levelMananger.ExitGame();
    }
}
