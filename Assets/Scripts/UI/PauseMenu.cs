using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsMenuPanel;
    public GameObject pauseMenuPanel;

    public TimeSystem time;



    public static PauseMenu instance;
    void Awake()
    {
        #region NotNullChecks
        if (settingsMenuPanel == null)
        {
            Debug.LogError("Settings Panel not found.");
        }
        if (pauseMenuPanel == null)
        {
            Debug.LogError("Pause Panel not found.");
        }
        if (time == null)
        {
            Debug.LogError("TimeSystem Script not found.");
        }
        #endregion
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (pauseMenuPanel.activeSelf)
            {
                time.StartTime();
                pauseMenuPanel.SetActive(false);
            }
            else if (settingsMenuPanel.activeSelf) Settings();
            else if (SceneManager.GetActiveScene().name != "MainMenuScene")
            {
                time.StopTime();
                pauseMenuPanel.SetActive(true);
            }
        }
    }

    public void NewGame()
    {
        SaveSystem.DataManager.instance.ResetGameData();
        pauseMenuPanel.SetActive(false);
        GameManager.instance.StartNewGame();
    }

    public void Settings()
    {
        if (!settingsMenuPanel.activeSelf)
        {
            settingsMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            settingsMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
        }
    }


    public void ExitGame()
    {
        SaveSystem.DataManager.instance.UpdateAndSaveToFile();
        Application.Quit();
    }

    public void BackToMain()
    {
        SaveSystem.DataManager.instance.UpdateAndSaveToFile();
        pauseMenuPanel.SetActive(false);
        SceneManager.LoadScene("MainMenuScene");
    }
}
