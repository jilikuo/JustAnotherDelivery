using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private readonly string StartingSceneName = "InventorySortingScene";
    private readonly string ContinueButtonName = "ContinueButton";
    private readonly string LoadPanelName = "LoadGameMenuPanel";
    private readonly string SettingsPanelName = "SettingsMenuPanel";

    public GameObject continueButton;
    public GameObject loadPanel;
    public GameObject settingsPanel;
    public GameObject activePanel;

    private bool hasSaveFile = false;

    void Awake()
    {
        #region NotNullChecks
        if (continueButton == null)
        {
            continueButton = GameObject.Find(ContinueButtonName);
            if (continueButton == null)
            {
                Debug.LogError("Continue Button not found.");
            }
        }

        if (loadPanel == null) {
            loadPanel = GameObject.Find(LoadPanelName);
            if (loadPanel == null)
            {
                Debug.LogError("Load Panel not found.");
            }
        }

        if (settingsPanel == null) {
            settingsPanel = GameObject.Find(SettingsPanelName);
            if (settingsPanel == null)
            {
                Debug.LogError("Settings Panel not found.");
            }
        }
        #endregion

        loadPanel.SetActive(false);
        settingsPanel.SetActive(false);
        HandleContinueButtonStatus();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(StartingSceneName);
    }

    public void ContinueGame()
    {
        //TODO: Continue from last saved file logic
        Debug.Log("You pressed the Continue Game button.");
    }

    public void LoadGame()
    {
        activePanel = loadPanel;
        loadPanel.SetActive(true);
    }

    public void Settings()
    {
        activePanel = settingsPanel;
        settingsPanel.SetActive(true);
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
