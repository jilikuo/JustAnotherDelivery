using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject creditsMenu;

    public void Start()
    {
        if (continueButton == null)
        {
            Debug.LogError("continueButton not set");
        }
        if (creditsMenu == null)
        {
            Debug.LogError("creditsMenu not set");
        }
        continueButton.SetActive(SaveSystem.DataManager.instance.HasLoadedGameData());
        creditsMenu.SetActive(false);
    }

    public void NewGame()
    {
        GameManager.instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.instance.ContinueGame();
    }

    public void ReturnToMainMenu()
    {
        GameManager.instance.LoadScene(GameManager.GameScene.MainMenuScene);
    }

    public void ShowSettings()
    {
        GameManager.instance.ShowSettingsMenu(true);
    }

    public void ShowCredits()
    {
        creditsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        GameManager.instance.ExitGame();
    }
}
