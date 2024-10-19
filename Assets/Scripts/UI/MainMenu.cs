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
        GameManager.instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.instance.ContinueGame();
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
