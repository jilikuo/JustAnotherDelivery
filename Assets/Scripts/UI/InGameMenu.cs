using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SaveSystem;

public class InGameMenu : MonoBehaviour
{
    private GameObject child;

    public void Start()
    {
        // This object should have a single child
        child = transform.GetChild(0).gameObject;
        Show(false);
    }

    public bool IsVisible()
    {
        return child.activeSelf;
    }

    public void Show(bool show = true)
    {
        child.SetActive(show);
    }

    public void ResumeGame()
    {
        Show(false);
    }

    public void NewGame()
    {
        Show(false);
        GameManager.instance.StartNewGame();
    }

    public void ShowSettings()
    {
        GameManager.instance.ShowSettingsMenu(true);
    }
    public void ReturnToMainMenu()
    {
        Show(false);
        GameManager.instance.LoadScene(GameManager.GameScene.MainMenuScene);
    }

    public void ExitGame()
    {
        Show(false);
        GameManager.instance.ExitGame();
    }
}
