using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string StartingSceneName = "InventorySortingScene";
    public GameObject continueButton;
    public GameObject loadMenuPanel;
    public GameObject settingsMenuPanel;
    public GameObject creditsMenuPanel;

    private GameObject activePanel;
    private bool hasSaveFile = false;

    void Awake()
    {
        #region NotNullChecks
        if (continueButton == null)
        {
            Debug.LogError("Continue Button not found.");
        }

        if (loadMenuPanel == null) {
            Debug.LogError("Load Panel not found.");
        }

        if (settingsMenuPanel == null) {
             Debug.LogError("Settings Panel not found.");
        }
        #endregion

        loadMenuPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        creditsMenuPanel.SetActive(false);
        HandleContinueButtonStatus();
    }

    public void NewGame()
    {
        GameManager.instance.StartNewGame();
    }

    public void ContinueGame()
    {
        //TODO: Continue from last saved file logic
        Debug.Log("You pressed the Continue Game button.");
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

    private void HandleContinueButtonStatus()
    {
        //if save file exists, hasSaveFile = true
        if (hasSaveFile)
        {
            continueButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            continueButton.GetComponent<Button>().interactable = false;
        }
    }
}
