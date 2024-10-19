using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;

public class GameMenu : MonoBehaviour
{
    public GameObject topPanel;
    public GameObject continueButton;
    public GameObject settingsMenuPanel;
    public GameObject creditsMenuPanel;


    private void Start()
    {
        if (continueButton != null)
        {
            continueButton.GetComponent<Button>().interactable = SaveSystem.DataManager.instance.HasLoadedGameData();
        }
        if (settingsMenuPanel != null)
        {
            settingsMenuPanel.SetActive(false);
        }
        if (creditsMenuPanel != null)
        {
            creditsMenuPanel.SetActive(false);
        }
    }

    public void Close()
    {
        topPanel.SetActive(false);
    }

    public void NewGame()
    {
        GameManager.instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.instance.ContinueGame();
    }

    public void MainMenu()
    {
        GameManager.instance.LoadScene(GameManager.GameScene.MainMenuScene);
    }

    public void Settings()
    {
        settingsMenuPanel.SetActive(true);
    }

    public void Credits()
    {
        creditsMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        GameManager.instance.ExitGame();
    }
}
