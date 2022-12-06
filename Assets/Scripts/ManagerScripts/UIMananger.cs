using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIMananger : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas settings;
    public Canvas gameplay;
    public Canvas pause;
    public Canvas credits;
    public Canvas gameOver;
    public Canvas win;

    // Sobre = On
    public void MainMenuCanvasOn()
    {
        mainMenu.enabled = true;
        settings.enabled = false;
        gameplay.enabled = false;
        pause.enabled = false;
        credits.enabled = false;
        gameOver.enabled = false;
        win.enabled = false;
    }

    public void SettingsCanvasOn()
    {
        mainMenu.enabled = false;
        settings.enabled = true;
        gameplay.enabled = false;
        pause.enabled = false;
        credits.enabled = false;
        gameOver.enabled = false;
        win.enabled = false;
    }

    public void GameplayCanvasOn()
    {
        mainMenu.enabled = false;
        settings.enabled = false;
        gameplay.enabled = true;
        pause.enabled = false;
        credits.enabled = false;
        gameOver.enabled = false;
        win.enabled = false;
    }

    public void PauseCanvasOn()
    {
        mainMenu.enabled = false;
        settings.enabled = false;
        gameplay.enabled = false;
        pause.enabled = true;
        credits.enabled = false;
        gameOver.enabled = false;
        win.enabled = false;
    }

    public void CreditsCanvasOn()
    {
        mainMenu.enabled = false;
        settings.enabled = false;
        gameplay.enabled = false;
        pause.enabled = false;
        credits.enabled = true;
        gameOver.enabled = false;
        win.enabled = false;
    }

    public void GameOverCanvasOn()
    {
        mainMenu.enabled = false;
        settings.enabled = false;
        gameplay.enabled = false;
        pause.enabled = false;
        credits.enabled = false;
        gameOver.enabled = true;
        win.enabled = false;
    }

    public void WinCanvasOn()
    {
        mainMenu.enabled = false;
        settings.enabled = false;
        gameplay.enabled = false;
        pause.enabled = false;
        credits.enabled = false;
        gameOver.enabled = false;
        win.enabled = true;
    }
}
