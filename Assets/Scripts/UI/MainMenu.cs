using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject settingsMenuPanel;
    public GameObject creditsMenuPanel;

    private GameObject activePanel;

    void Awake()
    {
        #region NotNullChecks
        if (continueButton == null)
        {
            Debug.LogError("Continue Button not found.");
        }

        if (settingsMenuPanel == null) {
             Debug.LogError("Settings Panel not found.");
        }
        #endregion
    }

    private void Start()
    {
        continueButton.GetComponent<Button>().interactable = SaveSystem.DataManager.instance.HasLoadedGameData();
        settingsMenuPanel.SetActive(false);
        creditsMenuPanel.SetActive(false);
    }

    public void NewGame()
    {
        SaveSystem.DataManager.instance.ResetGameData();
        GameManager.instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.instance.ContinueGame();
    }

    public void Settings()
    {
        settingsMenuPanel.SetActive(true);
        activePanel = settingsMenuPanel;
    }

    public void Credits()
    {
        creditsMenuPanel.SetActive(true);
        activePanel = creditsMenuPanel;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        activePanel.SetActive(false);
        activePanel = null;
    }
}
